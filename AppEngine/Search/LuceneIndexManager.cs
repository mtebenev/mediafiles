using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using Mt.MediaFiles.AppEngine.Common;
using Directory = System.IO.Directory;
using LDirectory = Lucene.Net.Store.Directory;

namespace Mt.MediaFiles.AppEngine.Search
{
  /// <summary>
  /// Provides methods to manage physical Lucene indices.
  /// This class is provided as a singleton to that the index searcher can be reused across requests.
  /// </summary>
  internal class LuceneIndexManager : IDisposable
  {
    private readonly IClock _clock;
    private readonly string _rootPath;
    private readonly IDirectoryInfo _rootDirectory;
    private bool _disposing;

    private readonly ConcurrentDictionary<string, IndexReaderPool> _indexPools;
    private readonly ConcurrentDictionary<string, IndexWriterWrapper> _writers;
    private readonly ConcurrentDictionary<string, DateTime> _timestamps;

    private static LuceneVersion LuceneVersion = LuceneVersion.LUCENE_48;

    public LuceneIndexManager(IClock clock, IFileSystem fileSystem, AppEngineSettings appEngineSettings)
    {
      _clock = clock;
      _rootPath = fileSystem.Path.Combine(appEngineSettings.DataDirectory, "lucene");
      _rootDirectory = fileSystem.Directory.CreateDirectory(_rootPath);

      _indexPools = new ConcurrentDictionary<string, IndexReaderPool>(StringComparer.OrdinalIgnoreCase);
      _writers = new ConcurrentDictionary<string, IndexWriterWrapper>(StringComparer.OrdinalIgnoreCase);
      _timestamps = new ConcurrentDictionary<string, DateTime>(StringComparer.OrdinalIgnoreCase);
    }

    public void CreateIndex(string indexName)
    {
      var path = new DirectoryInfo(Path.Combine(_rootPath, indexName));

      if(!path.Exists)
        path.Create();

      WriteIndex(indexName, _ => { }, true);
    }

    public void DeleteDocuments(string indexName, IEnumerable<string> catalogItemIds)
    {
      WriteIndex(indexName, writer =>
      {
        writer.DeleteDocuments(catalogItemIds.Select(x => new Term("CatalogItemId", x)).ToArray());
        writer.Commit();

        if(_indexPools.TryRemove(indexName, out var pool))
          pool.MakeDirty();
      });
    }

    public void DeleteIndex(string indexName)
    {
      lock(this)
      {
        if(_writers.TryGetValue(indexName, out var writer))
        {
          writer.IsClosing = true;
          writer.Dispose();
        }

        if(_indexPools.TryRemove(indexName, out var reader))
          reader.Dispose();

        _timestamps.TryRemove(indexName, out var timestamp);

        var indexFolder = Path.Combine(_rootPath, indexName);

        if(Directory.Exists(indexFolder))
        {
          try
          {
            Directory.Delete(indexFolder, true);
          }
          catch
          {
          }
        }

        _writers.TryRemove(indexName, out writer);
      }
    }

    /// <summary>
    /// Checks if a specified index exists
    /// </summary>
    public bool IsIndexExists(string indexName)
    {
      var result = false;

      if(!string.IsNullOrWhiteSpace(indexName))
        result = Directory.Exists(Path.Combine(_rootPath, indexName));

      return result;
    }

    public IEnumerable<string> List()
    {
      return _rootDirectory
        .GetDirectories()
        .Select(x => x.Name);
    }

    public void StoreDocuments(string indexName, IEnumerable<DocumentIndex> indexDocuments)
    {
      WriteIndex(indexName, writer =>
      {
        foreach(var indexDocument in indexDocuments)
          writer.AddDocument(CreateLuceneDocument(indexDocument));

        writer.Commit();

        if(_indexPools.TryRemove(indexName, out var pool))
          pool.MakeDirty();
      });
    }

    public async Task SearchAsync(string indexName, Func<IndexSearcher, Task> searcher)
    {
      using(var reader = GetReader(indexName))
      {
        var indexSearcher = new IndexSearcher(reader.IndexReader);
        await searcher(indexSearcher);
      }

      _timestamps[indexName] = _clock.UtcNow;
    }

    public void Read(string indexName, Action<IndexReader> reader)
    {
      using(var indexReader = GetReader(indexName))
      {
        reader(indexReader.IndexReader);
      }

      _timestamps[indexName] = _clock.UtcNow;
    }

    /// <summary>
    /// Returns a list of open indices and the last time they were accessed.
    /// </summary>
    public IReadOnlyDictionary<string, DateTime> GetTimestamps()
    {
      return new ReadOnlyDictionary<string, DateTime>(_timestamps);
    }

    private Document CreateLuceneDocument(DocumentIndex documentIndex)
    {
      var doc = new Document
      {
        // Always store the catalog item id
        new StringField("CatalogItemId", documentIndex.ItemId, Field.Store.YES)
      };

      foreach(var entry in documentIndex.Entries)
      {
        var store = entry.Value.IndexOptions.HasFlag(DocumentIndexOptions.Store)
          ? Field.Store.YES
          : Field.Store.NO;

        if(entry.Value.Value == null)
        {
          continue;
        }

        switch(entry.Value.IndexValueType)
        {
          case IndexValueType.Text:

            Field field = null;

            if(entry.Value.IndexOptions.HasFlag(DocumentIndexOptions.Analyze))
              field = new TextField(entry.Key, Convert.ToString(entry.Value.Value), store);
            else
              field = new StringField(entry.Key, Convert.ToString(entry.Value.Value), store);

            doc.Add(field);

            break;
        }
      }

      return doc;
    }

    private BaseDirectory CreateDirectory(string indexName)
    {
      lock(this)
      {
        var path = new DirectoryInfo(Path.Combine(_rootPath, indexName));

        if(!path.Exists)
          path.Create();

        return FSDirectory.Open(path);
      }
    }

    private void WriteIndex(string indexName, Action<IndexWriter> action, bool close = false)
    {
      if(!_writers.TryGetValue(indexName, out var writer))
      {
        lock(this)
        {
          if(!_writers.TryGetValue(indexName, out writer))
          {
            var directory = CreateDirectory(indexName);

            var config = new IndexWriterConfig(LuceneVersion, new StandardAnalyzer(LuceneVersion))
            {
              OpenMode = OpenMode.CREATE_OR_APPEND
            };

            writer = _writers[indexName] = new IndexWriterWrapper(directory, config);
          }
        }
      }

      if(writer.IsClosing)
        return;

      action?.Invoke(writer);

      if(close && !writer.IsClosing)
      {
        lock(this)
        {
          if(!writer.IsClosing)
          {
            writer.IsClosing = true;
            writer.Dispose();
            _writers.TryRemove(indexName, out writer);
          }
        }
      }

      _timestamps[indexName] = _clock.UtcNow;
    }

    private IndexReaderPool.IndexReaderLease GetReader(string indexName)
    {
      var pool = _indexPools.GetOrAdd(indexName, n =>
      {
        var directory = CreateDirectory(indexName);
        var reader = DirectoryReader.Open(directory);
        return new IndexReaderPool(reader);
      });

      return pool.Acquire();
    }

    /// <summary>
    /// Releases all readers and writers. This can be used after some time of innactivity to free resources.
    /// </summary>
    public void FreeReaderWriter()
    {
      lock(this)
      {
        foreach(var writer in _writers)
        {
          writer.Value.IsClosing = true;
          writer.Value.Dispose();
        }

        foreach(var reader in _indexPools)
          reader.Value.Dispose();

        _writers.Clear();
        _indexPools.Clear();
        _timestamps.Clear();
      }
    }

    /// <summary>
    /// Releases all readers and writers. This can be used after some time of innactivity to free resources.
    /// </summary>
    public void FreeReaderWriter(string indexName)
    {
      lock(this)
      {
        if(_writers.TryGetValue(indexName, out var writer))
        {
          writer.IsClosing = true;
          writer.Dispose();
        }

        if(_indexPools.TryGetValue(indexName, out var reader))
          reader.Dispose();

        _timestamps.TryRemove(indexName, out var timestamp);
        _writers.TryRemove(indexName, out writer);
      }
    }

    public void Dispose()
    {
      if(_disposing)
        return;

      _disposing = true;

      FreeReaderWriter();
    }

    ~LuceneIndexManager()
    {
      Dispose();
    }
  }
}
