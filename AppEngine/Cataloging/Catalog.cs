using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.QueryParsers.Simple;
using Lucene.Net.Search;
using Microsoft.Extensions.Logging;
using Mt.MediaMan.AppEngine.CatalogStorage;
using Mt.MediaMan.AppEngine.Common;
using Mt.MediaMan.AppEngine.Scanning;
using Mt.MediaMan.AppEngine.Search;

namespace Mt.MediaMan.AppEngine.Cataloging
{
  public class Catalog : IDisposable
  {
    private readonly IItemStorage _itemStorage;
    private readonly IStorageManager _storageManager;
    private readonly LuceneIndexManager _indexManager;
    private ICatalogItem _rootItem;

    /// <summary>
    /// Open/create catalog
    /// </summary>
    public static Catalog CreateCatalog(string connectionString)
    {
      var storageManager = new StorageManager(connectionString);
      var indexManager = new LuceneIndexManager(new Clock());
      var catalog = new Catalog(storageManager, indexManager);

      return catalog;
    }

    /// <summary>
    /// Reset catalog storage
    /// </summary>
    public static async Task ResetCatalogStorage(string connectionString)
    {
      await StorageManager.ResetStorage(connectionString);
      var indexManager = new LuceneIndexManager(new Clock());
      indexManager.DeleteIndex("default");
    }

    internal Catalog(IStorageManager storageManager, LuceneIndexManager indexManager)
    {
      _storageManager = storageManager;
      _indexManager = indexManager;
      _itemStorage = new ItemStorage(storageManager);
    }

    public ICatalogItem RootItem
    {
      get
      {
        if(_rootItem == null)
          throw new InvalidOperationException("OpenAsync() must be invoked to open catalog");

        return _rootItem;
      }
    }

    /// <summary>
    /// Use to determine if the catalog is open
    /// </summary>
    public bool IsOpen => _rootItem != null;

    /// <summary>
    /// IDisposable
    /// </summary>
    public void Dispose()
    {
      _storageManager?.Dispose();
    }

    /// <summary>
    /// Loads initial data from catalog (root item etc)
    /// </summary>
    public async Task OpenAsync(StorageConfiguration storageConfiguration)
    {
      await _itemStorage.InitializeAsync(storageConfiguration.ModuleStorageProviders);
      if(!_indexManager.IsIndexExists("default"))
        _indexManager.CreateIndex("default");

      // Root item
      var catalogItemRecord = await _itemStorage.LoadRootItemAsync();
      _rootItem = new CatalogItem(catalogItemRecord, _itemStorage);
    }

    /// <summary>
    /// Loads an item with specified ID
    /// </summary>
    public async Task<ICatalogItem> GetItemByIdAsync(int itemId)
    {
      var catalogItemRecord = await _itemStorage.LoadItemByIdAsync(itemId);
      var result = new CatalogItem(catalogItemRecord, _itemStorage);

      return result;
    }

    /// <summary>
    /// Scans new item to the catalog
    /// </summary>
    internal Task ScanAsync(ScanConfiguration scanConfiguration, IItemScanner itemScanner, ILoggerFactory loggerFactory)
    {
      // Create scan context
      var scanContext = new ScanContext(scanConfiguration, _itemStorage, _indexManager, loggerFactory);
      return itemScanner.Scan(scanContext);
    }

    /// <summary>
    /// Performs search and returns list of found item IDs
    /// TODO MTE: check how orchard performs search, it uses MultiFieldQueryParser
    /// </summary>
    internal async Task<IList<int>> SearchAsync(string query)
    {
      var catalogItemIdStrings = new List<string>();
      var idSet = new HashSet<string>(new[] {"CatalogItemId"});

      // TODO MTE: it works only for file names, need to check other analyzers
      var analyzer = new StandardAnalyzer(SearchConstants.LuceneVersion);
      var queryParser = new SimpleQueryParser(analyzer, "Book.Title");

      string escapedQuery = QueryParser.Escape(query);
      var luceneQuery = queryParser.Parse(escapedQuery);

      await _indexManager.SearchAsync("default", searcher =>
      {
        // Fetch one more result than PageSize to generate "More" links
        var collector = TopScoreDocCollector.Create(100, true);

        searcher.Search(luceneQuery, collector);
        var hits = collector.GetTopDocs(0);

        foreach(var hit in hits.ScoreDocs)
        {
          var d = searcher.Doc(hit.Doc);
          catalogItemIdStrings.Add(d.GetField("CatalogItemId").GetStringValue());
        }

        return Task.CompletedTask;
      });

      var result = catalogItemIdStrings
        .Select(idString => int.Parse(idString))
        .ToList();

      return result;
    }

    /// <summary>
    /// Search in item storage by file name, return item IDs
    /// </summary>
    internal Task<IList<int>> SearchFilesAsync(string query)
    {
      return _itemStorage.SearchItemsAsync(query);
    }

    public ModuleStorage CreateModuleStorage()
    {
      var result = new ModuleStorage(_storageManager.Store);
      return result;
    }
  }
}
