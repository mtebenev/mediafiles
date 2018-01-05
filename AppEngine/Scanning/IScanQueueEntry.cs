using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.CatalogStorage;

namespace Mt.MediaMan.AppEngine.Scanning
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
