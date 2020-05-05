using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using Mt.MediaFiles.AppEngine.FileHandlers;

namespace Mt.MediaFiles.AppEngine.Scanning
{
  /// <summary>
  /// Scanning sub-task for retrieving file properties with scan drivers.
  /// </summary>
  internal class ScanServiceScanInfo : IScanService
  {
    private readonly IList<IFileHandler> _fileHandlers;

    /// <summary>
    /// IScanService.
    /// </summary>
    public string Id => HandlerIds.ScanSvcScanInfo;

    public ScanServiceScanInfo(IEnumerable<IFileHandler> fileHandlers)
    {
      this._fileHandlers = fileHandlers.ToList();
    }

    /// <summary>
    /// IScanService.
    /// </summary>
    public async Task ScanAsync(IScanServiceContext context, CatalogItemRecord record)
    {
      if(record.ItemType == CatalogItemType.File)
      {
        var fsContext = await context.GetFileStoreEntryContextAsync();
        var fileHandlers = await this.GetSupportedFileHandlersAsync(record, fsContext);

        await this.RunScanDriversAsync(context, record, fsContext, fileHandlers);
      }
    }

    /// <summary>
    /// IScanService.
    /// </summary>
    public Task FlushAsync()
    {
      return Task.CompletedTask;
    }

    /// <summary>
    /// Determines all supported drivers for the scan entry
    /// TODO: make in parallel
    /// </summary>
    private async Task<IList<IFileHandler>> GetSupportedFileHandlersAsync(CatalogItemRecord record, FileStoreEntryContext fsContext)
    {
      var supportedHandlers = new List<IFileHandler>();
      foreach(var fileHandler in this._fileHandlers)
      {
        var isSupported = await fileHandler.IsSupportedAsync(fsContext.FileStoreEntry);
        if(isSupported)
          supportedHandlers.Add(fileHandler);
      }

      return supportedHandlers;
    }

    private async Task RunScanDriversAsync(IScanServiceContext context, CatalogItemRecord record, FileStoreEntryContext fsContext, IList<IFileHandler> fileHandlers)
    {
      var catalogItemData = context.GetItemData();

      // TODO: make in parallel
      foreach(var fileHandler in fileHandlers)
      {
        await RunSingleScanDriverAsync(context, record, fsContext, fileHandler, catalogItemData);
      }
    }

    private async Task RunSingleScanDriverAsync(IScanServiceContext context, CatalogItemRecord record, FileStoreEntryContext fsContext, IFileHandler fileHandler, CatalogItemData catalogItemData)
    {
      try
      {
        await fileHandler.ScanDriver.ScanAsync(
          record.CatalogItemId,
          fsContext,
          catalogItemData
          );
      }
      catch(Exception e)
      {
        context.Logger.LogError(e, $"Scan error file: {record.Path} ({record.CatalogItemId}): ");
      }
    }
  }
}
