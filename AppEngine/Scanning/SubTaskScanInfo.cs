using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.CatalogStorage;
using Mt.MediaMan.AppEngine.FileHandlers;
using Mt.MediaMan.AppEngine.FileStorage;
using Mt.MediaMan.AppEngine.Search;

namespace Mt.MediaMan.AppEngine.Scanning
{
  /// <summary>
  /// Scanning sub-task for retrieving file properties with scan drivers.
  /// </summary>
  internal class SubTaskScanInfo : ISubTask
  {
    public async Task ExecuteAsync(IScanContext scanContext, CatalogItemRecord record)
    {
      IList<IFileHandler> fileHandlers = new List<IFileHandler>();

      if(record.ItemType == CatalogItemType.File)
      {
        var directoryPath = Path.GetDirectoryName(record.Path);
        var fileName = Path.GetFileName(record.Path);

        var fsStore = new FileSystemStore(directoryPath);
        var fsEntry = await fsStore.GetFileInfoAsync(fileName);

        var fsContext = new FileStoreEntryContext(fsEntry, fsStore);
        fileHandlers = await this.GetSupportedFileHandlersAsync(scanContext, record, fsContext);

        // Run file handlers
        var catalogItemData = await this.RunScanDriversAsync(scanContext, record, fsContext, fileHandlers);
        await this.RunIndexersAsync(scanContext, record, fsContext, catalogItemData, fileHandlers);
        await scanContext.ItemStorage.SaveItemDataAsync(record.CatalogItemId, catalogItemData);
      }
    }

    /// <summary>
    /// Determines all supported drivers for the scan entry
    /// TODO: make in parallel
    /// </summary>
    private async Task<IList<IFileHandler>> GetSupportedFileHandlersAsync(IScanContext scanContext, CatalogItemRecord record, FileStoreEntryContext fsContext)
    {
      var supportedHandlers = new List<IFileHandler>();
      foreach(var fileHandler in scanContext.ScanConfiguration.FileHandlers)
      {
        var isSupported = await fileHandler.IsSupportedAsync(fsContext.FileStoreEntry);
        if(isSupported)
          supportedHandlers.Add(fileHandler);
      }

      return supportedHandlers;
    }

    private async Task<CatalogItemData> RunScanDriversAsync(IScanContext scanContext, CatalogItemRecord record, FileStoreEntryContext fsContext, IList<IFileHandler> fileHandlers)
    {
      var catalogItemData = new CatalogItemData(record.CatalogItemId);

      // TODO: make in parallel
      foreach(var fileHandler in fileHandlers)
      {
        await RunSingleScanDriverAsync(scanContext, record, fsContext, fileHandler, catalogItemData);
      }

      return catalogItemData;
    }

    private async Task RunSingleScanDriverAsync(IScanContext scanContext, CatalogItemRecord record, FileStoreEntryContext fsContext, IFileHandler fileHandler, CatalogItemData catalogItemData)
    {
      try
      {
        await fileHandler.ScanDriver.ScanAsync(
          scanContext,
          record.CatalogItemId,
          fsContext,
          catalogItemData
          );
      }
      catch(Exception e)
      {
        scanContext.Logger.LogError(e, $"Scan error file: {record.Path} ({record.CatalogItemId}): ");
      }
    }

    /// <summary>
    /// Runs indexers on the item.
    /// </summary>
    private async Task RunIndexersAsync(IScanContext scanContext, CatalogItemRecord record, FileStoreEntryContext fsContext, CatalogItemData catalogItemData, IList<IFileHandler> fileHandlers)
    {
      DocumentIndex documentIndex = new DocumentIndex(record.CatalogItemId.ToString());
      var indexingContext = new IndexingContext(documentIndex, record, catalogItemData);

      // Perform indexing
      foreach(var fileHandler in fileHandlers)
      {
        await fileHandler.CatalogItemIndexer.IndexItemAsync(indexingContext);
      }

      // Store index document
      foreach(var index in scanContext.IndexManager.List())
      {
        scanContext.IndexManager.DeleteDocuments(index, new string[] { indexingContext.ItemRecord.CatalogItemId.ToString() });
        scanContext.IndexManager.StoreDocuments(index, new DocumentIndex[] { indexingContext.DocumentIndex });
      }
    }
  }
}
