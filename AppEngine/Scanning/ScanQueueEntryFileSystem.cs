using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Cataloging;
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
    private readonly IScanContext _scanContext;
    private readonly int _parentItemId;
    private readonly IFileStore _fileStore;

    public ScanQueueEntryFileSystem(IScanContext scanContext, int parentItemId, IFileStore fileStore, IFileStoreEntry fileStoreEntry)
    {
      _catalogItemId = null;
      _parentItemId = parentItemId;
      _fileStore = fileStore;
      _fileStoreEntry = fileStoreEntry;
      _scanContext = scanContext;
    }


    /// <summary>
    /// IScanQueueEntry
    /// </summary>
    public async Task StoreAsync(IItemStorage itemStorage)
    {
      if(_catalogItemId.HasValue)
        throw new InvalidOperationException("Scan queue item already stored");

      // Common file properties
      var itemRecord = new CatalogItemRecord
      {
        Name = _fileStoreEntry.Name,
        Size = (int) _fileStoreEntry.Length,
        ParentItemId = _parentItemId,
        ItemType = _fileStoreEntry.IsDirectory ? CatalogItemType.Directory : CatalogItemType.File
      };

      IList<IScanDriver> drivers = Enumerable.Empty<IScanDriver>().ToList();
      if(itemRecord.ItemType == CatalogItemType.File)
        drivers = await GetSupportedDriversAsync();

      _catalogItemId = await itemStorage.CreateItemAsync(itemRecord);

      // TODO: make in parallel
      foreach(var scanDriver in drivers)
      {
        FileStoreEntryContext fileStoreEntryContext = new FileStoreEntryContext(_fileStoreEntry, _fileStore);
        await scanDriver.ScanAsync(_scanContext, _catalogItemId.Value, fileStoreEntryContext, itemStorage);
      }
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
          var childEntry = new ScanQueueEntryFileSystem(_scanContext, _catalogItemId.Value, _fileStore, childFileStoreEntry);
          scanQueue.Enqueue(childEntry);
        }
      }
    }

    /// <summary>
    /// Determines all supported drivers for the scan entry
    /// TODO: make in parallel
    /// </summary>
    private async Task<IList<IScanDriver>> GetSupportedDriversAsync()
    {
      var supportedDrivers = new List<IScanDriver>();
      foreach(var scanDriver in _scanContext.ScanDrivers)
      {
        var isSupported = await scanDriver.IsSupportedAsync(_fileStoreEntry);
        if(isSupported)
          supportedDrivers.Add(scanDriver);
      }

      return supportedDrivers;
    }
  }
}
