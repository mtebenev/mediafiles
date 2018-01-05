using System.Threading.Tasks;

namespace Mt.MediaMan.AppEngine.CatalogStorage
{
  internal interface IItemStorage
  {
    /// <summary>
    /// Saves a new item in the storage. Returns ID of the saved record
    /// </summary>
    Task<int> CreateItem(CatalogItemRecord itemRecord);
  }
}
