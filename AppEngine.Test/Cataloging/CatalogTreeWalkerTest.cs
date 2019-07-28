using Mt.MediaMan.AppEngine.Cataloging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mt.MediaMan.AppEngine.Test.Cataloging
{
  public class CatalogTreeWalkerTest
  {
    [Fact]
    public async Task Enumerate_Items()
    {
      var mockChildItem1 = Substitute.For<ICatalogItem>();
      mockChildItem1.GetChildrenAsync().Returns(new List<ICatalogItem>());
      mockChildItem1.CatalogItemId.Returns(2);


      var mockChildItem2 = Substitute.For<ICatalogItem>();
      mockChildItem2.GetChildrenAsync().Returns(new List<ICatalogItem>());
      mockChildItem2.CatalogItemId.Returns(3);

      IList<ICatalogItem> childrenList = new List<ICatalogItem>()
      {
        mockChildItem1,
        mockChildItem2
      };

      var mockCatalogItem = Substitute.For<ICatalogItem>();
      mockCatalogItem.GetChildrenAsync().Returns(childrenList);
      mockCatalogItem.CatalogItemId.Returns(1);

      var mockCatalog = Substitute.For<ICatalog>();
      mockCatalog.GetItemByIdAsync(1).Returns(mockCatalogItem);
      mockCatalog.GetItemByIdAsync(2).Returns(mockChildItem1);
      mockCatalog.GetItemByIdAsync(3).Returns(mockChildItem2);

      var enumerable = CatalogTreeWalker.CreateDefaultWalker(mockCatalog, 1);
      var mockAction = Substitute.For<Action<ICatalogItem>>();

      await enumerable.ForEachAsync(mockAction);

      mockAction.Received()(Arg.Is<ICatalogItem>(x => x.CatalogItemId == 1));
      mockAction.Received()(Arg.Is<ICatalogItem>(x => x.CatalogItemId == 2));
      mockAction.Received()(Arg.Is<ICatalogItem>(x => x.CatalogItemId == 3));
      mockAction.Received(3)(Arg.Any<ICatalogItem>());
    }
  }
}
