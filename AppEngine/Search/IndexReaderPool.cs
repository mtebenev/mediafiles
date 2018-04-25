using System;
using System.Threading;
using Lucene.Net.Index;

namespace Mt.MediaMan.AppEngine.Search
{
  /// <summary>
  /// TODO MTE: update from Orchard
  /// </summary>
  internal class IndexReaderPool : IDisposable
  {
    private bool _dirty;
    private int _count;
    private readonly IndexReader _reader;

    public IndexReaderPool(IndexReader reader)
    {
      _reader = reader;
    }

    public void MakeDirty()
    {
      _dirty = true;
    }

    public IndexReaderLease Acquire()
    {
      return new IndexReaderLease(this, _reader);
    }

    public void Release()
    {
      if(_dirty && _count == 0)
      {
        _reader.Dispose();
      }
    }

    public void Dispose()
    {
      _reader.Dispose();
    }

    public struct IndexReaderLease : IDisposable
    {
      private readonly IndexReaderPool _pool;

      public IndexReaderLease(IndexReaderPool pool, IndexReader reader)
      {
        _pool = pool;
        Interlocked.Increment(ref _pool._count);
        IndexReader = reader;
      }

      public IndexReader IndexReader { get; }

      public void Dispose()
      {
        _pool.Release();
      }
    }
  }
}
