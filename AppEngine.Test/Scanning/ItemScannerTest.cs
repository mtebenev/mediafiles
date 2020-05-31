using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using Mt.MediaFiles.AppEngine.Scanning;
using NSubstitute;
using Xunit;

namespace Mt.MediaFiles.AppEngine.Test.Scanning
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

      var sut = new ItemScanner(
        mockLoggerFactory,
        mockItemExplorer,
        new List<IBufferedStorage>(),
        1,
        @"x:\folder1\folder2");
      await sut.Scan(mockScanContext);

      // Verify
      await mockScanContext.ItemStorage.Received(1).SaveItemDataAsync(
        SCAN_ROOT_ID,
        Arg.Is<CatalogItemData>(cd => cd.Get<InfoPartScanRoot>().RootPath == @"some_root_path"));

    }

    [Fact]
    public async Task Should_Invoke_Scan_Services()
    {
      var mockServices = new List<IScanService>
      {
        Substitute.For<IScanService>(),
        Substitute.For<IScanService>()
      };

      var mockLoggerFactory = Substitute.For<ILoggerFactory>();
      var mockItemExplorer = Substitute.For<IItemExplorer>();

      var mockScanContext = Substitute.For<IScanContext>();
      mockScanContext.ScanConfiguration.ScanServices.Returns(mockServices);

      var records = new List<CatalogItemRecord>
      {
        new CatalogItemRecord {CatalogItemId = 100},
        new CatalogItemRecord {CatalogItemId = 101},
        new CatalogItemRecord {CatalogItemId = 102},
      };

      mockScanContext.ItemStorage.QuerySubtree(default).ReturnsForAnyArgs(records);

      var infoPartScanRoot = new InfoPartScanRoot { RootPath = "some_root_path" };
      mockItemExplorer.CreateScanRootPartAsync(@"x:\folder1\folder2").Returns(infoPartScanRoot);

      var sut = new ItemScanner(
        mockLoggerFactory,
        mockItemExplorer,
        new List<IBufferedStorage>(),
        1,
        @"x:\folder1\folder2");
      await sut.Scan(mockScanContext);

      // Verify
      foreach(var s in mockServices)
      {
        foreach(var r in records)
        {
          await s.Received().ScanAsync(Arg.Any<IScanServiceContext>(), r);
        }
      }
    }

    [Fact]
    public async Task Should_Flush_Buffers()
    {
      var mockBufferedStorages = new List<IBufferedStorage>
      {
        Substitute.For<IBufferedStorage>(),
        Substitute.For<IBufferedStorage>()
      };

      var mockLoggerFactory = Substitute.For<ILoggerFactory>();
      var mockItemExplorer = Substitute.For<IItemExplorer>();

      var mockScanContext = Substitute.For<IScanContext>();


      var infoPartScanRoot = new InfoPartScanRoot { RootPath = "some_root_path" };
      mockItemExplorer.CreateScanRootPartAsync(@"x:\folder1\folder2").Returns(infoPartScanRoot);

      var sut = new ItemScanner(
        mockLoggerFactory,
        mockItemExplorer,
        mockBufferedStorages,
        1,
        @"x:\folder1\folder2");
      await sut.Scan(mockScanContext);

      // Verify
      foreach(var bs in mockBufferedStorages)
      {
        await bs.Received().FlushAsync();
      }
    }
  }
}
