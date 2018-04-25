using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.CatalogStorage;
using Mt.MediaMan.AppEngine.FileHandlers;
using Mt.MediaMan.AppEngine.Search;
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

      IList<IFileHandler> fileHandlers = new List<IFileHandler>();

      if(itemRecord.ItemType == CatalogItemType.File)
        fileHandlers = await GetSupportedFileHandlersAsync();

      _catalogItemId = await itemStorage.CreateItemAsync(itemRecord);

      // Drivers
      var catalogItemData = await RunScanDriversAsync(fileHandlers);
      await itemStorage.SaveItemDataAsync(_catalogItemId.Value, catalogItemData);

      // Indexers
      await RunItemIndexersAsync(fileHandlers, itemRecord, catalogItemData);
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
    private async Task<IList<IFileHandler>> GetSupportedFileHandlersAsync()
    {
      var supportedHandlers = new List<IFileHandler>();
      foreach(var fileHandler in _scanContext.ScanConfiguration.FileHandlers)
      {
        var isSupported = await fileHandler.IsSupportedAsync(_fileStoreEntry);
        if(isSupported)
          supportedHandlers.Add(fileHandler);
      }

      return supportedHandlers;
    }

    private async Task<CatalogItemData> RunScanDriversAsync(IList<IFileHandler> fileHandlers)
    {
      var catalogItemData = new CatalogItemData(_catalogItemId.Value);

      // TODO: make in parallel
      foreach(var fileHandler in fileHandlers)
      {
        FileStoreEntryContext fileStoreEntryContext = new FileStoreEntryContext(_fileStoreEntry, _fileStore);
        await fileHandler.ScanDriver.ScanAsync(_scanContext, _catalogItemId.Value, fileStoreEntryContext, catalogItemData);
      }

      return catalogItemData;
    }

    private async Task RunItemIndexersAsync(IList<IFileHandler> fileHandlers, CatalogItemRecord itemRecord, CatalogItemData catalogItemData)
    {
      DocumentIndex documentIndex = new DocumentIndex(itemRecord.CatalogItemId.ToString());
      var indexingContext = new IndexingContext(documentIndex, itemRecord, catalogItemData);

      // Perform indexing
      foreach(var fileHandler in fileHandlers)
      {
        await fileHandler.CatalogItemIndexer.IndexItemAsync(indexingContext);
      }

      // Store index document
      foreach(var index in _scanContext.IndexManager.List())
      {
        _scanContext.IndexManager.DeleteDocuments(index, new string[] {indexingContext.ItemRecord.CatalogItemId.ToString()});
        _scanContext.IndexManager.StoreDocuments(index, new DocumentIndex[] {indexingContext.DocumentIndex});
      }
    }

  }
}
