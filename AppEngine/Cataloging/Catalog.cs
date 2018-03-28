using System;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.CatalogStorage;
using Mt.MediaMan.AppEngine.Scanning;

namespace Mt.MediaMan.AppEngine.Cataloging
{
  public class Catalog : IDisposable
  {
    private readonly IItemStorage _itemStorage;
    private ICatalogItem _rootItem;

    public static Catalog CreateCatalog(string connectionString)
    {
      var itemStorage = new ItemStorage(connectionString);
      var catalog = new Catalog(itemStorage);

      return catalog;
    }

    internal Catalog(IItemStorage itemStorage)
    {
      _itemStorage = itemStorage;
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
    /// Call to initialize necessary data in new catalog database
    /// </summary>
    public async Task InitializeNewCatalogAsync()
    {
      await _itemStorage.InitializeAsync();
      await SaveRootItemAsync();
      await OpenAsync();
    }

    /// <summary>
    /// Loads initial data from catalog (root item etc)
    /// </summary>
    public async Task OpenAsync()
    {
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
    internal Task ScanAsync(IItemScanner itemScanner)
    {
      return itemScanner.Scan(_itemStorage);
    }

    private Task SaveRootItemAsync()
    {
      var catalogItem = new CatalogItemRecord
      {
        ItemType = CatalogItemType.CatalogRoot,
        Name = "[ROOT]"
      };

      return _itemStorage.CreateItemAsync(catalogItem);
    }
  }
}
