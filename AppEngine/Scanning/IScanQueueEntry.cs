using System.Threading.Tasks;
using Mt.MediaFiles.AppEngine.CatalogStorage;

namespace Mt.MediaFiles.AppEngine.Scanning
{
  internal interface IScanQueueEntry
  {
    /// <summary>
    /// Use to store the item in catalog
    /// </summary>
    Task StoreAsync(IItemStorage itemStorage);

    Task EnqueueChildrenAsync(IScanQueue scanQueue);
  }
}
