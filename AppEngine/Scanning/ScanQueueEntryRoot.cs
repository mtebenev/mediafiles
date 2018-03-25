using System;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.CatalogStorage;
using OrchardCore.FileStorage;

namespace Mt.MediaMan.AppEngine.Scanning
{
  /// <summary>
  /// Root entry - the entry firstly added to catalog when scanning (information origin as known in xtdb)
  /// </summary>
  internal class ScanQueueEntryRoot : IScanQueueEntry
  {
    private int? _catalogItemId;
    private readonly IFileStore _fileStore;
    private readonly int _parentItemId;

    public ScanQueueEntryRoot(IFileStore fileStore, int parentItemId)
    {
      _catalogItemId = null;
      _fileStore = fileStore;
      _parentItemId = parentItemId;
    }

    public async Task StoreAsync(IItemStorage itemStorage)
    {
      if(_catalogItemId.HasValue)
        throw new InvalidOperationException("Scan queue item already stored");

      var itemRecord = new CatalogItemRecord
      {
        Name = "[SCAN_ROOT]",
        Size = 0,
        ParentItemId = _parentItemId,
        ItemType = CatalogItemType.ScanRoot
      };

      _catalogItemId = await itemStorage.CreateItemAsync(itemRecord);
    }

    public async Task EnqueueChildrenAsync(IScanQueue scanQueue)
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
