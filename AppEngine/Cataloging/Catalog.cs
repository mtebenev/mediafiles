using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mt.MediaMan.AppEngine.CatalogStorage;
using Mt.MediaMan.AppEngine.Common;
using Mt.MediaMan.AppEngine.Search;
using Mt.MediaMan.AppEngine.Tasks;

namespace Mt.MediaMan.AppEngine.Cataloging
{
  /// <summary>
  /// The catalog implementation.
  /// </summary>
  public class Catalog : ICatalog
  {
    private readonly IItemStorage _itemStorage;
    private readonly IStorageManager _storageManager;
    private readonly LuceneIndexManager _indexManager;
    private ICatalogItem _rootItem;

    /// <summary>
    /// Open/create catalog
    /// </summary>
    public static Catalog CreateCatalog(IServiceProvider serviceProvider)
    {
      var catalogSettings = serviceProvider.GetRequiredService<ICatalogSettings>();
      var storageManager = serviceProvider.GetRequiredService<IStorageManager>();

      var indexManager = new LuceneIndexManager(new Clock());
      var catalog = new Catalog(catalogSettings.CatalogName, storageManager, indexManager);

      return catalog;
    }

    internal Catalog(string catalogName, IStorageManager storageManager, LuceneIndexManager indexManager)
    {
      CatalogName = catalogName;
      _storageManager = storageManager;
      _indexManager = indexManager;
      _itemStorage = new ItemStorage(storageManager);
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
    public async Task OpenAsync(StorageConfiguration storageConfiguration)
    {
      await _itemStorage.InitializeAsync(storageConfiguration.ModuleStorageProviders);

      foreach(var mdbp in storageConfiguration.ModuleDbProviders)
      {
        await mdbp.InitializeDbAsync(this._storageManager.DbConnection);
      }

      if(!_indexManager.IsIndexExists(CatalogName))
        _indexManager.CreateIndex(CatalogName);

      // Root item
      var catalogItemRecord = await _itemStorage.LoadRootItemAsync();
      _rootItem = new CatalogItem(catalogItemRecord, _itemStorage);
    }

    /// <summary>
    /// ICatalog
    /// </summary>
    public async Task<ICatalogItem> GetItemByIdAsync(int itemId)
    {
      var catalogItemRecord = await _itemStorage.LoadItemByIdAsync(itemId);
      var result = new CatalogItem(catalogItemRecord, _itemStorage);

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
    public void Close()
    {
      if(_indexManager == null)
        throw new InvalidOperationException("Catalog is not open");

      _rootItem = null;
    }

    /// <summary>
    /// The item storage.
    /// </summary>
    internal IItemStorage ItemStorage => this._itemStorage;

    /// <summary>
    /// The index manager.
    /// </summary>
    internal LuceneIndexManager IndexManager => this._indexManager;

    /// <summary>
    /// Search in item storage by file name, return item IDs
    /// </summary>
    internal Task<IList<int>> SearchFilesAsync(string query)
    {
      return _itemStorage.SearchItemsAsync(query);
    }

    public ModuleStorage CreateModuleStorage()
    {
      var result = new ModuleStorage(_storageManager.Store);
      return result;
    }
  }
}
