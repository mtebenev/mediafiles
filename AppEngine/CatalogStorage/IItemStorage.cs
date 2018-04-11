using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mt.MediaMan.AppEngine.CatalogStorage
{
  internal interface IItemStorage : IDisposable
  {
    /// <summary>
    /// Invoke the first thing to initialize document storage
    /// </summary>
    Task InitializeAsync();

    /// <summary>
    /// Saves a new item in the storage. Returns ID of the saved record
    /// </summary>
    Task<int> CreateItemAsync(CatalogItemRecord itemRecord);

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
  }
}
