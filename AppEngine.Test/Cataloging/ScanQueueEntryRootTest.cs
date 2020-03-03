using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.CatalogStorage;
using Mt.MediaMan.AppEngine.Scanning;
using NSubstitute;
using OrchardCore.FileStorage;
using Xunit;

namespace Mt.MediaMan.AppEngine.Test.Cataloging
{
  public class ScanQueueEntryRootTest
  {
    [Fact]
    public async Task Should_Save_Scan_Root_Info_Part()
    {
      var mockScanDirectoryEntry = Substitute.For<IFileStoreEntry>();
      mockScanDirectoryEntry.AccessFileAsync().Returns(@"C:\scan_root_folder");

      var mockFileStore = Substitute.For<IFileStore>();
      mockFileStore.GetDirectoryInfoAsync("").Returns(mockScanDirectoryEntry);

      var mockScanContext = Substitute.For<IScanContext>();
      var mockItemStorage = Substitute.For<IItemStorage>();
      mockItemStorage.SaveItemDataAsync(Arg.Any<int>(), Arg.Any<CatalogItemData>()).Returns(Task.CompletedTask);
      var scanQueueEntry = new ScanQueueEntryRoot(mockScanContext, mockFileStore, 1);
      await scanQueueEntry.StoreAsync(mockItemStorage);

      // Verify
      await mockItemStorage.Received(1).SaveItemDataAsync(
        Arg.Any<int>(),
        Arg.Is<CatalogItemData>(cd => cd.Get<InfoPartScanRoot>().RootPath == @"C:\scan_root_folder"));
    }
  }
}
