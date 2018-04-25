using System;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.CatalogStorage;
using Mt.MediaMan.AppEngine.Common;
using Mt.MediaMan.AppEngine.Scanning;
using Mt.MediaMan.AppEngine.Search;

namespace Mt.MediaMan.AppEngine.Cataloging
{
  public class Catalog : IDisposable
  {
    private readonly IItemStorage _itemStorage;
    private readonly LuceneIndexManager _indexManager;
    private ICatalogItem _rootItem;

    public static Catalog CreateCatalog(string connectionString)
    {
      var itemStorage = new ItemStorage(connectionString);
      var indexManager = new LuceneIndexManager(new Clock());
      var catalog = new Catalog(itemStorage, indexManager);

      return catalog;
    }

    internal Catalog(IItemStorage itemStorage, LuceneIndexManager indexManager)
    {
      _itemStorage = itemStorage;
      _indexManager = indexManager;
    }

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
      _itemStorage?.Dispose();
    }

    /// <summary>
    /// Loads initial data from catalog (root item etc)
    /// </summary>
    public async Task OpenAsync()
    {
      await _itemStorage.InitializeAsync();
      if(!_indexManager.IsIndexExists("default"))
        _indexManager.CreateIndex("default");

      // Root item
      var catalogItemRecord = await _itemStorage.LoadRootItemAsync();
      _rootItem = new CatalogItem(catalogItemRecord, _itemStorage);
    }

    /// <summary>
    /// Loads an item with specified ID
    /// </summary>
    public async Task<ICatalogItem> GetItemByIdAsync(int itemId)
    {
      var catalogItemRecord = await _itemStorage.LoadItemByIdAsync(itemId);
      var result = new CatalogItem(catalogItemRecord, _itemStorage);

      return result;
    }

    /// <summary>
    /// Scans new item to the catalog
    /// </summary>
    internal Task ScanAsync(ScanConfiguration scanConfiguration, IItemScanner itemScanner)
    {
      // Create scan context
      var scanContext = new ScanContext(scanConfiguration, _itemStorage, _indexManager);
      return itemScanner.Scan(scanContext);
    }
  }
}
