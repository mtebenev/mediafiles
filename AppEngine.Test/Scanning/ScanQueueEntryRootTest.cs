using Mt.MediaMan.AppEngine.CatalogStorage;
using Mt.MediaMan.AppEngine.FileStorage;
using Mt.MediaMan.AppEngine.Scanning;
using NSubstitute;
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

      var sut = new ScanQueueEntryRoot(mockScanContext, mockFileStore, 1);
      await sut.StoreAsync(mockItemStorage);
      await sut.EnqueueChildrenAsync(mockScanQueue);

      // Verify
      mockScanConfiguration.Received().IsIgnoredEntry("Child1");
      mockScanConfiguration.Received().IsIgnoredEntry("Child2");
      mockScanConfiguration.Received().IsIgnoredEntry("Child3");
      mockScanQueue.Received(2).Enqueue(Arg.Any<IScanQueueEntry>()); // Should recieve "Child2", "Child3"
    }
  }
}
