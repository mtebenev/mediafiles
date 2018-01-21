using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Catalog;
using Mt.MediaMan.AppEngine.CatalogStorage;
using Mt.MediaMan.AppEngine.Scanning;

namespace Mt.MediaMan.AppEngine.Cataloging
{
  public class Catalog
  {
    private readonly IItemStorage _itemStorage;

    public static Catalog CreateCatalog()
    {
      var itemStorage = new ItemStorage();
      var catalog = new Catalog(itemStorage);

      return catalog;
    }

    internal Catalog(IItemStorage itemStorage)
    {
      _itemStorage = itemStorage;
      RootItem = new CatalogItem();
    }

    public ICatalogItem RootItem { get; private set; }

    /// <summary>
    /// Scans new item to the catalog
    /// </summary>
    internal Task ScanAsync(IItemScanner itemScanner)
    {
      return itemScanner.Scan(_itemStorage);
    }
  }
}
