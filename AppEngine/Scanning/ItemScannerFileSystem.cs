using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mt.MediaMan.AppEngine.Cataloging;
using OrchardCore.FileStorage;

namespace Mt.MediaMan.AppEngine.Scanning
{
  internal class ItemScannerFileSystem : IItemScanner
  {
    private readonly IFileStore _fileStore;
    private readonly ICatalogItem _parentItem;
    private readonly IScanQueue _scanQueue;
    private readonly ILogger<ItemScannerFileSystem> _logger;

    public ItemScannerFileSystem(IFileStore fileStore, ICatalogItem parentItem, IScanQueue scanQueue, ILoggerFactory loggerFactory)
    {
      _fileStore = fileStore;
      _parentItem = parentItem;
      _scanQueue = scanQueue;
      _logger = loggerFactory.CreateLogger<ItemScannerFileSystem>();
    }

    public async Task Scan(IScanContext scanContext)
    {
      _logger.LogInformation("Scanning started");
      InitQueue(scanContext);
      
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
      var rootScanEntry = new ScanQueueEntryRoot(scanContext, _fileStore, _parentItem.CatalogItemId);
      _scanQueue.Enqueue(rootScanEntry);
    }
  }
}
