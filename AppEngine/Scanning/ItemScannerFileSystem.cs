using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.CatalogStorage;
using OrchardCore.FileStorage;

namespace Mt.MediaMan.AppEngine.Scanning
{
  internal class ItemScannerFileSystem : IItemScanner
  {
    private readonly IFileStore _fileStore;
    private readonly IScanQueue _scanQueue;

    public ItemScannerFileSystem(IFileStore fileStore, IScanQueue scanQueue)
    {
      _fileStore = fileStore;
      _scanQueue = scanQueue;
    }

    public async Task Scan(IItemStorage itemStorage)
    {
      InitQueue();
      
      do
      {
        var scanQueueEntry = _scanQueue.Dequeue();
        await scanQueueEntry.StoreAsync(itemStorage);

        await scanQueueEntry.EnqueueChildrenAsync(_scanQueue);
      } while(_scanQueue.Count > 0);
    }

    private void InitQueue()
    {
      var rootScanEntry = new ScanQueueEntryRoot(_fileStore);
      _scanQueue.Enqueue(rootScanEntry);
    }
  }
}
