using Xunit;

namespace Mt.MediaMan.AppEngine.Test.Catalog
{
  public class CatalogTest
  {
    [Fact]
    public void New_Catalog_Should_Provide_Root_Item()
    {
      var catalog = new AppEngine.Catalog.Catalog();
      var rootItem = catalog.RootItem;

      Assert.NotNull(rootItem);
    }
  }
}