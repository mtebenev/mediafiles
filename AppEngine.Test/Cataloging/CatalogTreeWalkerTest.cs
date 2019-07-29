using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Test.TestUtils;
using NSubstitute;
using System;
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
      var catalogDef = @"
{
  name: 'Root',
  children: [
    {name: 'Item 1'},
    {name: 'Item 2'},
    {name: 'Item 3'},
  ]
}
";

      var mockCatalog = CatalogMockBuilder.Create(catalogDef).Build();

      var enumerable = CatalogTreeWalker.CreateDefaultWalker(mockCatalog, 1);
      var mockAction = Substitute.For<Action<ICatalogItem>>();

      await enumerable.ForEachAsync(mockAction);

      mockAction.Received()(Arg.Is<ICatalogItem>(x => x.Name == "Root"));
      mockAction.Received()(Arg.Is<ICatalogItem>(x => x.Name == "Item 1"));
      mockAction.Received()(Arg.Is<ICatalogItem>(x => x.Name == "Item 2"));
      mockAction.Received()(Arg.Is<ICatalogItem>(x => x.Name == "Item 3"));
      mockAction.Received(4)(Arg.Any<ICatalogItem>());
    }
  }
}
