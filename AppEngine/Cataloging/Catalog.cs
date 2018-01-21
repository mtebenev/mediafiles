using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Catalog;
using Mt.MediaMan.AppEngine.Scanning;

namespace Mt.MediaMan.AppEngine.Cataloging
{
  public class Catalog
  {
    public Catalog()
    {
      RootItem = new CatalogItem();
      
    }

    public ICatalogItem RootItem { get; private set; }

    /// <summary>
    /// Scans new item to the catalog
    /// </summary>
    internal Task ScanAsync(IItemScanner path)
    {
      return Task.CompletedTask;
    }
  }
}
