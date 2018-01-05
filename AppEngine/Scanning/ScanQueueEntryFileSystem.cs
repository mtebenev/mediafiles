using System;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.CatalogStorage;
using OrchardCore.FileStorage;

namespace Mt.MediaMan.AppEngine.Scanning
{
  /// <summary>
  /// Scan queue entry for FS files/folders
  /// </summary>
  internal class ScanQueueEntryFileSystem : IScanQueueEntry
  {
    private int? _catalogItemId;
    private readonly IFileStoreEntry _fileStoreEntry;
    private int _parentItemId;
    private readonly IFileStore _fileStore;

    public ScanQueueEntryFileSystem(int parentItemId, IFileStore fileStore, IFileStoreEntry fileStoreEntry)
    {
      _catalogItemId = null;
      _parentItemId = parentItemId;
      _fileStore = fileStore;
      _fileStoreEntry = fileStoreEntry;
    }


    /// <summary>
    /// IScanQueueEntry
    /// </summary>
    public async Task StoreAsync(IItemStorage itemStorage)
    {
      if(_catalogItemId.HasValue)
        throw new InvalidOperationException("Scan queue item already stored");

      var itemRecord = new CatalogItemRecord
      {
        Name = _fileStoreEntry.Name,
        Size = 0,
        ParentItemId = _parentItemId
      };

      _catalogItemId = await itemStorage.CreateItem(itemRecord);
    }

    /// <summary>
    /// IScanQueueEntry
    /// </summary>
    public async Task EnqueueChildrenAsync(IScanQueue scanQueue)
    {
      if(!_catalogItemId.HasValue)
        throw new InvalidOperationException("Scan queue item must be stored before enqueuing children");

      if(_fileStoreEntry.IsDirectory)
      {
        var childEntries = await _fileStore.GetDirectoryContentAsync(_fileStoreEntry.Path);
        foreach(var childFileStoreEntry in childEntries)
        {
          var childEntry = new ScanQueueEntryFileSystem(_catalogItemId.Value, _fileStore, childFileStoreEntry);
          scanQueue.Enqueue(childEntry);
        }
      }
    }
  }
}
