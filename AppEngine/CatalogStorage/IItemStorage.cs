using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mt.MediaMan.AppEngine.CatalogStorage
{
  internal interface IItemStorage
  {
    /// <summary>
    /// Invoke the first thing to initialize document storage
    /// </summary>
    Task InitializeAsync(IReadOnlyList<IModuleStorageProvider> moduleStorageProviders);

    /// <summary>
    /// Saves a new item in the storage. Returns ID of the saved record
    /// </summary>
    Task<int> CreateItemAsync(CatalogItemRecord itemRecord);

    /// <summary>
    /// Saves multiple items in the storage.
    /// </summary>
    Task CreateManyItemsAsync(IEnumerable<CatalogItemRecord> records);

    /// <summary>
    /// Loads root item in the storage
    /// </summary>
    Task<CatalogItemRecord> LoadRootItemAsync();

    /// <summary>
    /// Loads a specific item record
    /// </summary>
    Task<CatalogItemRecord> LoadItemByIdAsync(int catalogItemId);

    /// <summary>
    /// Loads children records of the specified item
    /// </summary>
    Task<IList<CatalogItemRecord>> LoadChildrenAsync(int catalogItemId);

    /// <summary>
    /// Stores info part associated with a catalog item
    /// </summary>
    Task SaveItemDataAsync(int catalogItemId, CatalogItemData itemData);

    /// <summary>
    /// Loads info part associated with a catalog item
    /// </summary>
    Task<CatalogItemData> LoadItemDataAsync(int catalogItemId);

    /// <summary>
    /// Search items by name (with wildcards)
    /// </summary>
    Task<IList<int>> SearchItemsAsync(string whereFilter);
  }
}
