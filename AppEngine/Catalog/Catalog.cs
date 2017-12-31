namespace Mt.MediaMan.AppEngine.Catalog
{
  public class Catalog
  {
    public Catalog()
    {
      RootItem = new CatalogItem();
      
    }

    public ICatalogItem RootItem { get; private set; }
  }
}
