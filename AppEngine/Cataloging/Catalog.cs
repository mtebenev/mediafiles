using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.CatalogStorage;
using Mt.MediaMan.AppEngine.Search;
using Mt.MediaMan.AppEngine.Tasks;

namespace Mt.MediaMan.AppEngine.Cataloging
{
  /// <summary>
  /// The catalog implementation.
  /// </summary>
  internal class Catalog : ICatalog
  {
    private readonly IStorageManager _storageManager;
    private ICatalogItem _rootItem;

    internal Catalog(string catalogName, IStorageManager storageManager, LuceneIndexManager indexManager)
    {
      this.CatalogName = catalogName;
      this._storageManager = storageManager;
      this.IndexManager = indexManager;
      this.ItemStorage = new ItemStorage(storageManager);
    }

    /// <summary>
    /// ICatalog.
    /// </summary>
    public string CatalogName { get; }

    /// <summary>
    /// ICatalog.
    /// </summary>
    public ICatalogItem RootItem
    {
      get
      {
        if(_rootItem == null)
          throw new InvalidOperationException("OpenAsync() must be invoked to open catalog");

        return _rootItem;
      }
    }

    /// <summary>
    /// Use to determine if the catalog is open
    /// </summary>
    public bool IsOpen => _rootItem != null;

    /// <summary>
    /// IDisposable
    /// </summary>
    public void Dispose()
    {
      _storageManager?.Dispose();
    }

    /// <summary>
    /// Loads initial data from catalog (root item etc)
    /// </summary>
    internal async Task OpenAsync(StorageConfiguration storageConfiguration)
    {
      await ItemStorage.InitializeAsync(storageConfiguration.ModuleStorageProviders);

      foreach(var mdbp in storageConfiguration.ModuleDbProviders)
      {
        await mdbp.InitializeDbAsync(this._storageManager.DbConnection);
      }

      if(!IndexManager.IsIndexExists(CatalogName))
        IndexManager.CreateIndex(CatalogName);

      // Root item
      var catalogItemRecord = await ItemStorage.LoadRootItemAsync();
      _rootItem = new CatalogItem(catalogItemRecord, ItemStorage);
    }

    /// <summary>
    /// ICatalog
    /// </summary>
    public async Task<ICatalogItem> GetItemByIdAsync(int itemId)
    {
      var catalogItemRecord = await ItemStorage.LoadItemByIdAsync(itemId);
      var result = new CatalogItem(catalogItemRecord, ItemStorage);

      return result;
    }

    /// <summary>
    /// ICatalog.
    /// </summary>
    public Task ExecuteTaskAsync(CatalogTaskBase catalogTask)
    {
      return catalogTask.ExecuteAsync(this);
    }

    /// <summary>
    /// ICatalog.
    /// </summary>
    public Task<TResult> ExecuteTaskAsync<TResult>(CatalogTaskBase<TResult> catalogTask)
    {
      return catalogTask.ExecuteAsync(this);
    }

    /// <summary>
    /// ICatalog.
    /// </summary>
    Task ICatalog.ExecuteTaskAsync(IInternalCatalogTask task)
    {
      return task.ExecuteAsync(this);
    }

    /// <summary>
    /// ICatalog.
    /// </summary>
    Task<TResult> ICatalog.ExecuteTaskAsync<TResult>(IInternalCatalogTask<TResult> task)
    {
      return task.ExecuteAsync(this);
    }

    /// <summary>
    /// ICatalog.
    /// </summary>
    public void Close()
    {
      if(IndexManager == null)
        throw new InvalidOperationException("Catalog is not open");

      _rootItem = null;
    }

    /// <summary>
    /// The item storage.
    /// </summary>
    internal IItemStorage ItemStorage { get; }

    /// <summary>
    /// The index manager.
    /// </summary>
    internal LuceneIndexManager IndexManager { get; }

    /// <summary>
    /// Search in item storage by file name, return item IDs
    /// </summary>
    internal Task<IList<int>> SearchFilesAsync(string query)
    {
      return ItemStorage.SearchItemsAsync(query);
    }

    public ModuleStorage CreateModuleStorage()
    {
      var result = new ModuleStorage(_storageManager.Store);
      return result;
    }
  }
}
