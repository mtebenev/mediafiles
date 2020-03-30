using System.IO.Abstractions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.FileStorage;

namespace Mt.MediaMan.AppEngine.Scanning
{
  public interface IItemScannerFileSystemFactory
  {
    internal IItemScanner Create(IFileStore fileStore, ICatalogItem parentItem, IScanQueue scanQueue);
  }

  internal class ItemScannerFileSystem : IItemScanner
  {
    private readonly IFileStore _fileStore;
    private readonly IFileSystem _fileSystem;
    private readonly ICatalogItem _parentItem;
    private readonly IScanQueue _scanQueue;
    private readonly ILogger<ItemScannerFileSystem> _logger;

    public ItemScannerFileSystem(IFileSystem fileSystem, ILoggerFactory loggerFactory, IFileStore fileStore, ICatalogItem parentItem, IScanQueue scanQueue)
    {
      this._fileStore = fileStore;
      this._fileSystem = fileSystem;
      this._parentItem = parentItem;
      this._scanQueue = scanQueue;
      this._logger = loggerFactory.CreateLogger<ItemScannerFileSystem>();
    }

    public async Task Scan(IScanContext scanContext)
    {
      this._logger.LogInformation("Scanning started");
      this.InitQueue(scanContext);
      
      do
      {
        var scanQueueEntry = _scanQueue.Dequeue();
        await scanQueueEntry.StoreAsync(scanContext.ItemStorage);

        await scanQueueEntry.EnqueueChildrenAsync(_scanQueue);
      } while(_scanQueue.Count > 0);

      _logger.LogInformation("Scanning finished");
    }

    private void InitQueue(IScanContext scanContext)
    {
      var rootScanEntry = new ScanQueueEntryRoot(scanContext, _fileStore, this._fileSystem, _parentItem.CatalogItemId);
      _scanQueue.Enqueue(rootScanEntry);
    }
  }
}
