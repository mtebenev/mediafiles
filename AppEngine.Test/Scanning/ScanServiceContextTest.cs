using System;
using System.Threading.Tasks;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using Mt.MediaFiles.AppEngine.Scanning;
using NSubstitute;
using Xunit;

namespace Mt.MediaFiles.AppEngine.Test.Scanning
{
  public class ScanServiceContextTest
  {
    [Fact]
    public void Should_Throw_When_Get_Data_Without_Record()
    {
      var mockScanContext = Substitute.For<IScanContext>();

      var sut = new ScanServiceContext(mockScanContext);
      Assert.Throws<InvalidOperationException>(() =>
      {
        sut.GetItemData();
      });
    }

    [Fact]
    public async Task Should_Save_Item_Data()
    {
      var mockItemStorage = Substitute.For<IItemStorage>();
      var mockScanContext = Substitute.For<IScanContext>();
      var record = new CatalogItemRecord { CatalogItemId = 100 };

      var sut = new ScanServiceContext(mockScanContext);
      sut.SetCurrentRecord(record);
      var catalogItemData = sut.GetItemData();

      await sut.SaveDataAsync(mockItemStorage);

      // Verify
      await mockItemStorage.Received().SaveItemDataAsync(100, catalogItemData);
    }

    [Fact]
    public async Task Should_Not_Save_Item_Data_If_Item_Data_Not_Created()
    {
      var mockItemStorage = Substitute.For<IItemStorage>();
      var mockScanContext = Substitute.For<IScanContext>();
      var record = new CatalogItemRecord { CatalogItemId = 100 };

      var sut = new ScanServiceContext(mockScanContext);
      sut.SetCurrentRecord(record);

      await sut.SaveDataAsync(mockItemStorage);

      // Verify
      await mockItemStorage.DidNotReceive().SaveItemDataAsync(Arg.Any<int>(), Arg.Any<CatalogItemData>());
    }
  }
}
