using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using Mt.MediaMan.AppEngine.CatalogStorage;
using Mt.MediaMan.AppEngine.Scanning;
using OrchardCore.FileStorage;
using Xunit;

namespace Mt.MediaMan.AppEngine.Test.Cataloging
{
  public class ScanQueueEntryRootTest
  {
    [Fact]
    public async Task Should_Save_Scan_Root_Info_Part()
    {
      var fixture = new Fixture()
        .Customize(new AutoMoqCustomization {ConfigureMembers = true});

      var mockScanDirectoryEntry = new Mock<IFileStoreEntry>();
      mockScanDirectoryEntry
        .Setup(x => x.AccessFileAsync())
        .ReturnsAsync(@"C:\scan_root_folder");

      var mockFileStore = fixture.Freeze<Mock<IFileStore>>();
      mockFileStore.Setup(x => x.GetDirectoryInfoAsync(""))
        .ReturnsAsync(mockScanDirectoryEntry.Object);

      var mockItemStorage = fixture.Freeze<Mock<IItemStorage>>();

      var scanQueueEntry = fixture.Create<ScanQueueEntryRoot>();
      var itemStorage = fixture.Create<IItemStorage>();
      await scanQueueEntry.StoreAsync(itemStorage);

      // Verify
      mockItemStorage.Verify(x => x.SaveItemDataAsync(
        It.IsAny<int>(),
        It.Is<CatalogItemData>(cd => cd.Get<InfoPartScanRoot>().RootPath == @"C:\scan_root_folder")), Times.Once);

    }
  }
}
