using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using Mt.MediaFiles.AppEngine.FileStorage;
using Mt.MediaFiles.AppEngine.Tasks;

namespace Mt.MediaFiles.AppEngine.Scanning
{
  /// <summary>
  /// Scan service context implementation.
  /// </summary>
  internal class ScanServiceContext : IScanServiceContext
  {
    private CatalogItemData _catalogItemData;
    private FileStoreEntryContext _fileStoreEntryContext;
    private CatalogItemRecord _currentRecord;

    /// <summary>
    /// Ctor.
    /// </summary>
    public ScanServiceContext(IScanContext scanContext)
    {
      this.Logger = scanContext.Logger;
      this.ProgressOperation = scanContext.ProgressOperation;
    }

    /// <summary>
    /// IScanServiceContext.
    /// </summary>
    public ILogger Logger { get; }

    /// <summary>
    /// IScanServiceContext.
    /// </summary>
    public IProgressOperation ProgressOperation { get; }

    /// <summary>
    /// IScanServiceContext.
    /// </summary>
    public async Task<FileStoreEntryContext> GetFileStoreEntryContextAsync()
    {
      if(this._fileStoreEntryContext == null)
      {
        var directoryPath = Path.GetDirectoryName(this._currentRecord.Path);
        var fileName = Path.GetFileName(this._currentRecord.Path);

        var fsStore = new FileSystemStore(directoryPath);
        var fsEntry = await fsStore.GetFileInfoAsync(fileName);

        this._fileStoreEntryContext = new FileStoreEntryContext(fsEntry, fsStore);
      }

      return this._fileStoreEntryContext;
    }

    /// <summary>
    /// IScanServiceContext.
    /// </summary>
    public CatalogItemData GetItemData()
    {
      if(this._currentRecord == null)
        throw new InvalidOperationException("Attempt to get item data without record");

      if(this._catalogItemData == null)
        this._catalogItemData = new CatalogItemData(this._currentRecord.CatalogItemId);

      return this._catalogItemData;
    }

    /// <summary>
    /// Resets the context with the new item record.
    /// </summary>
    internal void SetCurrentRecord(CatalogItemRecord r)
    {
      this._currentRecord = r;
      this._catalogItemData = null;
      this._fileStoreEntryContext = null;
    }

    /// <summary>
    /// Saves the current item state (if changed).
    /// </summary>
    internal Task SaveDataAsync(IItemStorage itemStorage)
    {
      var result = this._catalogItemData != null
          ? itemStorage.SaveItemDataAsync(this._currentRecord.CatalogItemId, this._catalogItemData)
          : Task.CompletedTask;

      return result;
    }
  }
}
