using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.CatalogStorage;
using OrchardCore.FileStorage;

namespace Mt.MediaMan.AppEngine.Catalog
{
  /// <summary>
  /// Root entry - the entry firstly added to catalog (information origin as known in xtdb)
  /// </summary>
  internal class ScanQueueEntryRoot : IScanQueueEntry
  {
    private int? _catalogItemId;
    private readonly IFileStore _fileStore;

    public ScanQueueEntryRoot(IFileStore fileStore)
    {
      _catalogItemId = null;
      _fileStore = fileStore;
    }

    public async Task StoreAsync(IItemStorage itemStorage)
    {
      if(_catalogItemId.HasValue)
        throw new InvalidOperationException("Scan queue item already stored");

      var itemRecord = new CatalogItemRecord
      {
        Name = "[ROOT]",
        Size = 0,
        ParentItemId = 0
      };

      _catalogItemId = await itemStorage.CreateItem(itemRecord);
    }

    public async Task EnqueueChildrenAsync(Queue<IScanQueueEntry> scanQueue)
    {
      if(!_catalogItemId.HasValue)
        throw new InvalidOperationException("Scan queue item must be stored before enqueuing children");

      var entries = await _fileStore.GetDirectoryContentAsync(null);

      foreach(var entry in entries)
      {
        var childEntry = new ScanQueueEntryFileSystem(_catalogItemId.Value, _fileStore, entry);
        scanQueue.Enqueue(childEntry);
      }
    }
  }
}
