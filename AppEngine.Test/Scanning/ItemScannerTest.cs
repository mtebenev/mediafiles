using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mt.MediaMan.AppEngine.CatalogStorage;
using Mt.MediaMan.AppEngine.Scanning;
using NSubstitute;
using Xunit;

namespace Mt.MediaMan.AppEngine.Test.Scanning
{
  public class ItemScannerTest
  {
    [Fact]
    public async Task Should_Store_Scan_Root_Data()
    {
      const int ROOT_ITEM_ID = 1;
      const int SCAN_ROOT_ID = 10;

      var mockLoggerFactory = Substitute.For<ILoggerFactory>();
      var mockItemExplorer = Substitute.For<IItemExplorer>();
      var mockScanContext = Substitute.For<IScanContext>();

      mockScanContext.ItemStorage.CreateItemAsync(Arg.Is<CatalogItemRecord>(x => x.ParentItemId == ROOT_ITEM_ID)).Returns(SCAN_ROOT_ID);
      var infoPartScanRoot = new InfoPartScanRoot { RootPath = "some_root_path" };
      mockItemExplorer.CreateScanRootPartAsync(@"x:\folder1\folder2").Returns(infoPartScanRoot);

      var sut = new ItemScanner(mockLoggerFactory, mockItemExplorer, 1, @"x:\folder1\folder2");
      await sut.Scan(mockScanContext);

      // Verify
      await mockScanContext.ItemStorage.Received(1).SaveItemDataAsync(
        SCAN_ROOT_ID,
        Arg.Is<CatalogItemData>(cd => cd.Get<InfoPartScanRoot>().RootPath == @"some_root_path"));

    }
  }
}
