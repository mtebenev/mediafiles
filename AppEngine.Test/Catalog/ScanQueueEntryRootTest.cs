using System.Threading.Tasks;
using Moq;
using Mt.MediaMan.AppEngine.CatalogStorage;
using Mt.MediaMan.AppEngine.Scanning;
using OrchardCore.FileStorage;
using Xunit;

namespace Mt.MediaMan.AppEngine.Test.Catalog
{
  public class ScanQueueEntryRootTest
  {
    /// <summary>
    /// During scan the children items must have parent ID set to root item ID
    /// </summary>
    [Fact]
    public async Task Assign_Parent_Id_To_Children()
    {
      var mockScanQueue = new Mock<IScanQueue>();
      var mockFileStore = CreateTestFileStore();
      var mockItemStorage = new Mock<IItemStorage>();

      var rootQueueEntry = new ScanQueueEntryRoot(mockFileStore.Object);

      await rootQueueEntry.StoreAsync(mockItemStorage.Object);
      await rootQueueEntry.EnqueueChildrenAsync(mockScanQueue.Object);

      // Must enque two children items
      mockScanQueue.Verify(x => x.Enqueue(It.IsAny<IScanQueueEntry>()), Times.Exactly(2));
    }

    private Mock<IFileStore> CreateTestFileStore()
    {
      var mockFile1 = new Mock<IFileStoreEntry>();
      mockFile1.SetupGet(x => x.Name).Returns("File1");

      var mockFile2 = new Mock<IFileStoreEntry>();
      mockFile2.SetupGet(x => x.Name).Returns("File2");

      var mockFileStore = new Mock<IFileStore>();
      mockFileStore.Setup(x => x.GetDirectoryContentAsync(null)).ReturnsAsync(new IFileStoreEntry[] {mockFile1.Object, mockFile2.Object});

      return mockFileStore;
    }

  }
}
