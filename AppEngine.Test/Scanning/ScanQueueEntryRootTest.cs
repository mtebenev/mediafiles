using Mt.MediaMan.AppEngine.CatalogStorage;
using Mt.MediaMan.AppEngine.FileStorage;
using Mt.MediaMan.AppEngine.Scanning;
using NSubstitute;
using System.IO.Abstractions;
using System.Threading.Tasks;
using Xunit;

namespace Mt.MediaMan.AppEngine.Test.Scanning
{
  public class ScanQueueEntryRootTest
  {
    [Fact]
    public async Task Should_Ignore_Items()
    {
      var mockScanContext = Substitute.For<IScanContext>();
      var mockFileStore = Substitute.For<IFileStore>();
      var mockFs = Substitute.For<IFileSystem>();
      var mockDriveInfo = Substitute.For<IDriveInfo>();
      mockDriveInfo.Name.Returns(@"C:\");
      mockFs.DriveInfo.GetDrives().Returns(new[] { mockDriveInfo });

      var mockFileStoreEntry = Substitute.For<IFileStoreEntry>();
      mockFileStoreEntry.AccessFileAsync().Returns(@"C:\some_path");

      var mockChildEntries = new[]
      {
        Substitute.For<IFileStoreEntry>(),
        Substitute.For<IFileStoreEntry>(),
        Substitute.For<IFileStoreEntry>(),
      };
      mockChildEntries[0].Name.Returns("Child1");
      mockChildEntries[1].Name.Returns("Child2");
      mockChildEntries[2].Name.Returns("Child3");

      mockFileStore.GetDirectoryInfoAsync("").Returns(mockFileStoreEntry);
      mockFileStore.GetDirectoryContentAsync(null).Returns(mockChildEntries);


      var mockScanQueue = Substitute.For<IScanQueue>();
      var mockItemStorage = Substitute.For<IItemStorage>();

      var mockScanConfiguration = Substitute.For<IScanConfiguration>();
      mockScanConfiguration.IsIgnoredEntry("Child1").Returns(true);
      mockScanConfiguration.IsIgnoredEntry("Child2").Returns(true);
      mockScanConfiguration.IsIgnoredEntry("Child2").Returns(false);
      
      mockScanContext.ScanConfiguration.Returns(mockScanConfiguration);

      var sut = new ScanQueueEntryRoot(mockScanContext, mockFileStore, mockFs, 1);
      await sut.StoreAsync(mockItemStorage);
      await sut.EnqueueChildrenAsync(mockScanQueue);

      // Verify
      mockScanConfiguration.Received().IsIgnoredEntry("Child1");
      mockScanConfiguration.Received().IsIgnoredEntry("Child2");
      mockScanConfiguration.Received().IsIgnoredEntry("Child3");
      mockScanQueue.Received(2).Enqueue(Arg.Any<IScanQueueEntry>()); // Should recieve "Child2", "Child3"
    }

    [Fact]
    public async Task Should_Save_Scan_Root_Info_Part()
    {
      var mockScanDirectoryEntry = Substitute.For<IFileStoreEntry>();
      mockScanDirectoryEntry.AccessFileAsync().Returns(@"c:\scan_root_folder");

      var mockFileStore = Substitute.For<IFileStore>();
      mockFileStore.GetDirectoryInfoAsync("").Returns(mockScanDirectoryEntry);

      var mockScanContext = Substitute.For<IScanContext>();
      var mockItemStorage = Substitute.For<IItemStorage>();
      var mockFs = Substitute.For<IFileSystem>();
      var mockDriveInfo = Substitute.For<IDriveInfo>();
      mockDriveInfo.Name.Returns(@"C:\");
      mockFs.DriveInfo.GetDrives().Returns(new[] { mockDriveInfo });

      mockItemStorage.SaveItemDataAsync(Arg.Any<int>(), Arg.Any<CatalogItemData>()).Returns(Task.CompletedTask);
      var scanQueueEntry = new ScanQueueEntryRoot(mockScanContext, mockFileStore, mockFs, 1);
      await scanQueueEntry.StoreAsync(mockItemStorage);

      // Verify
      await mockItemStorage.Received(1).SaveItemDataAsync(
        Arg.Any<int>(),
        Arg.Is<CatalogItemData>(cd => cd.Get<InfoPartScanRoot>().RootPath == @"c:\scan_root_folder"));
    }
  }
}
