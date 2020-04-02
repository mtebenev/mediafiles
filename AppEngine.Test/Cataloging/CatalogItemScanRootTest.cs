using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.CatalogStorage;
using Mt.MediaMan.AppEngine.Scanning;
using NSubstitute;
using Xunit;

namespace Mt.MediaMan.AppEngine.Test.Cataloging
{
  public class CatalogItemScanRootTest
  {
    [Fact]
    public async Task Should_Return_Fs_Children()
    {
      var itemId = 10;
      var record = new CatalogItemRecord
      {
        CatalogItemId = itemId
      };

      var itemData = new CatalogItemData(10);
      itemData.Apply<InfoPartScanRoot>(new InfoPartScanRoot
      {
        RootPath = @"x:\folder1"
      });

      var childRecords = new[]
      {
        new CatalogItemRecord { CatalogItemId = 21, ItemType = CatalogItemType.File },
        new CatalogItemRecord { CatalogItemId = 22, ItemType = CatalogItemType.File },
        new CatalogItemRecord { CatalogItemId = 23, ItemType = CatalogItemType.File },
      };

      var mockAccess = Substitute.For<IStructureAccess>();
      mockAccess.QueryLevelAsync(
        Arg.Is<CatalogItemLocation>(l => l.ScanRootId == itemId && l.PathPrefix == @"x:\folder1"))
        .Returns(childRecords);

      var mockItemStorage = Substitute.For<IItemStorage>();
      mockItemStorage.LoadItemDataAsync(itemId).Returns(itemData);

      var mockItemFactory = Substitute.For<ICatalogItemFactory>();
      var mockAccessFactory = Substitute.For<IStructureAccessFactory>();
      mockAccessFactory.CreateAsync(itemId).Returns(mockAccess);

      var item = new CatalogItemScanRoot(record, mockItemStorage, mockItemFactory, mockAccessFactory);

      var children = await item.GetChildrenAsync();

      // Verify
      mockItemFactory.Received().CreateItem(Arg.Is<CatalogItemRecord>(x => x.CatalogItemId == 21));
      mockItemFactory.Received().CreateItem(Arg.Is<CatalogItemRecord>(x => x.CatalogItemId == 22));
      mockItemFactory.Received().CreateItem(Arg.Is<CatalogItemRecord>(x => x.CatalogItemId == 23));
    }

    [Fact]
    public async Task Should_Return_Virtual_Children()
    {
      var itemId = 10;
      var record = new CatalogItemRecord
      {
        CatalogItemId = itemId
      };

      var itemData = new CatalogItemData(10);
      itemData.Apply<InfoPartScanRoot>(new InfoPartScanRoot
      {
        RootPath = @"x:\folder1"
      });

      var childRecords = new[]
      {
        new CatalogItemRecord { CatalogItemId = 21, ItemType = CatalogItemType.VirtualFolder, Path = "folder21" },
        new CatalogItemRecord { CatalogItemId = 22, ItemType = CatalogItemType.VirtualFolder, Path = "folder22" },
        new CatalogItemRecord { CatalogItemId = 23, ItemType = CatalogItemType.VirtualFolder, Path = "folder23" },
      };

      var mockAccess = Substitute.For<IStructureAccess>();
      mockAccess.QueryLevelAsync(
        Arg.Is<CatalogItemLocation>(l => l.ScanRootId == itemId && l.PathPrefix == @"x:\folder1"))
        .Returns(childRecords);

      var mockItemStorage = Substitute.For<IItemStorage>();
      mockItemStorage.LoadItemDataAsync(itemId).Returns(itemData);

      var mockItemFactory = Substitute.For<ICatalogItemFactory>();
      var mockAccessFactory = Substitute.For<IStructureAccessFactory>();
      mockAccessFactory.CreateAsync(itemId).Returns(mockAccess);

      var item = new CatalogItemScanRoot(record, mockItemStorage, mockItemFactory, mockAccessFactory);

      var children = await item.GetChildrenAsync();

      // Verify
      mockItemFactory.DidNotReceive().CreateItem(Arg.Any<CatalogItemRecord>());

      Assert.IsType<CatalogItemVirtualFolder>(children[0]);
      Assert.IsType<CatalogItemVirtualFolder>(children[1]);
      Assert.IsType<CatalogItemVirtualFolder>(children[2]);

      children
        .Select(c => new { c.CatalogItemId, c.Path })
        .Should()
        .BeEquivalentTo(new[]
        {
          new { CatalogItemId = 21, Path = "folder21" },
          new { CatalogItemId = 22, Path = "folder22" },
          new { CatalogItemId = 23, Path = "folder23" },
        });
    }
  }
}
