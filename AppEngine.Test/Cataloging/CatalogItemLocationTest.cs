using Mt.MediaMan.AppEngine.Cataloging;
using Xunit;

namespace Mt.MediaMan.AppEngine.Test.Cataloging
{
  public class CatalogItemLocationTest
  {
    [Fact]
    public void Should_Create_Child_Location()
    {
      var location = new CatalogItemLocation(10, @"x:\folder1\folder2");
      var childLocation = location.CreateChildLocation("folder3");

      Assert.Equal(@"x:\folder1\folder2\folder3", childLocation.PathPrefix);
      Assert.Equal(10, childLocation.ScanRootId);
    }
  }
}
