using System;
using System.IO;
using System.Linq;
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
    private readonly IScanContext _scanContext;
    private readonly IFileStore _fileStore;
    private readonly int _parentItemId;

    public ScanQueueEntryRoot(IScanContext scanContext, IFileStore fileStore, int parentItemId)
    {
      _catalogItemId = null;
      _scanContext = scanContext;
      _fileStore = fileStore;
      _parentItemId = parentItemId;
    }

    public async Task StoreAsync(IItemStorage itemStorage)
    {
      if(_catalogItemId.HasValue)
        throw new InvalidOperationException("Scan queue item already stored");

      var itemRecord = new CatalogItemRecord
      {
        Name = String.IsNullOrWhiteSpace(_scanContext.ScanConfiguration.ScanRootItemName) ? "[SCAN_ROOT]" : _scanContext.ScanConfiguration.ScanRootItemName,
        Size = 0,
        ParentItemId = _parentItemId,
        ItemType = CatalogItemType.ScanRoot
      };

      _catalogItemId = await itemStorage.CreateItemAsync(itemRecord);
      await StoreScanRootPart(itemStorage);
    }

    public async Task EnqueueChildrenAsync(IScanQueue scanQueue)
    {
      if(!_catalogItemId.HasValue)
        throw new InvalidOperationException("Scan queue item must be stored before enqueuing children");

      var entries = await _fileStore.GetDirectoryContentAsync(null);

      foreach(var entry in entries)
      {
        var childEntry = new ScanQueueEntryFileSystem(_scanContext, _catalogItemId.Value, _fileStore, entry);
        scanQueue.Enqueue(childEntry);
      }
    }

    /// <summary>
    /// Stores catalog item data for scan root
    /// </summary>
    private async Task StoreScanRootPart(IItemStorage itemStorage)
    {
      var catalogItemData = new CatalogItemData(_catalogItemId.Value);

      var fileStoreEntry = await _fileStore.GetDirectoryInfoAsync("");
      var fileStoreEntryContext = new FileStoreEntryContext(fileStoreEntry, _fileStore);
      var filePath = await fileStoreEntryContext.AccessFilePathAsync();
      var filePathRoot = Path.GetPathRoot(filePath);
      var infoPartScanRoot = catalogItemData.GetOrCreate<InfoPartScanRoot>();

      // Check for UNC
      var pathUri = new Uri(filePathRoot);
      if (pathUri.IsUnc)
        infoPartScanRoot.DriveType = DriveType.Network.ToString();
      else
      {
        var drives = DriveInfo.GetDrives();
        var driveInfo = drives.First(di => di.RootDirectory.FullName == filePathRoot);
        infoPartScanRoot.DriveType = driveInfo.DriveType.ToString();
      }

      infoPartScanRoot.RootPath = filePath;
      catalogItemData.Apply(infoPartScanRoot);

      // TODO: write IMAPI worker to determine media type

      await itemStorage.SaveItemDataAsync(_catalogItemId.Value, catalogItemData);
    }
  }
}
