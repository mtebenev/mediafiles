using System.Collections.Generic;
using System.Threading.Tasks;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using Mt.MediaFiles.AppEngine.FileHandlers;
using Mt.MediaFiles.AppEngine.FileStorage;
using Mt.MediaFiles.AppEngine.Scanning;
using NSubstitute;
using Xunit;

namespace Mt.MediaFiles.AppEngine.Test.Scanning
{
  public class ScanServiceScanInfoTest
  {
    [Fact]
    public async Task Should_Run_Only_Supported_Drivers()
    {
      var fileHandlers = new List<IFileHandler>
      {
        Substitute.For<IFileHandler>(),
        Substitute.For<IFileHandler>(),
        Substitute.For<IFileHandler>(),
        Substitute.For<IFileHandler>(),
      };

      fileHandlers[0].IsSupportedAsync(default).ReturnsForAnyArgs(false);
      fileHandlers[1].IsSupportedAsync(default).ReturnsForAnyArgs(false);
      fileHandlers[2].IsSupportedAsync(default).ReturnsForAnyArgs(true);
      fileHandlers[3].IsSupportedAsync(default).ReturnsForAnyArgs(true);

      var fileStoreEntryContext =
        new FileStoreEntryContext(
          Substitute.For<IFileStoreEntry>(),
          Substitute.For<IFileStore>()
      );

      var catalogItemData = new CatalogItemData(100);
      var mockContext = Substitute.For<IScanServiceContext>();
      mockContext.GetItemData().Returns(catalogItemData);
      mockContext.GetFileStoreEntryContextAsync().Returns(fileStoreEntryContext);
      var record = new CatalogItemRecord { CatalogItemId = 100, ItemType = CatalogItemType.File };

      var service = new ScanServiceScanInfo(fileHandlers);
      await service.ScanAsync(mockContext, record);

      // Verify
      await fileHandlers[0].ScanDriver.DidNotReceiveWithAnyArgs().ScanAsync(default, default, default);
      await fileHandlers[1].ScanDriver.DidNotReceiveWithAnyArgs().ScanAsync(default, default, default);
      await fileHandlers[2].ScanDriver.Received(1).ScanAsync(100, fileStoreEntryContext, catalogItemData);
      await fileHandlers[3].ScanDriver.Received(1).ScanAsync(100, fileStoreEntryContext, catalogItemData);
    }
  }
}
