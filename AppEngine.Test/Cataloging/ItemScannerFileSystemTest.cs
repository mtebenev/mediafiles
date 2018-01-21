using System.Threading.Tasks;
using Moq;
using Mt.MediaMan.AppEngine.CatalogStorage;
using Mt.MediaMan.AppEngine.Scanning;
using OrchardCore.FileStorage;
using Xunit;

namespace Mt.MediaMan.AppEngine.Test.Catalog
{
  public class ItemScannerFileSystemTest
  {
    [Fact]
    public async Task Must_Store_Items()
    {
      var fileStore = CreateTestFileStore();
      var mockItemStorage = new Mock<IItemStorage>();
      var scanQueue = new ScanQueue();
      mockItemStorage.Setup(x => x.CreateItem(It.Is<CatalogItemRecord>(r => r.Name == "[ROOT]"))).ReturnsAsync(1);

      var scanner = new ItemScannerFileSystem(fileStore, mockItemStorage.Object, scanQueue);

      await scanner.Scan(TODO);

      mockItemStorage.Verify(x => x.CreateItem(It.Is<CatalogItemRecord>(r => r.ParentItemId == 0)), Times.Once);
      mockItemStorage.Verify(x => x.CreateItem(It.Is<CatalogItemRecord>(r => r.ParentItemId == 1 && r.Name == "File1")), Times.Once);
      mockItemStorage.Verify(x => x.CreateItem(It.Is<CatalogItemRecord>(r => r.ParentItemId == 1 && r.Name == "File2")), Times.Once);
    }

    private IFileStore CreateTestFileStore()
    {
      var mockFile1 = new Mock<IFileStoreEntry>();
      mockFile1.SetupGet(x => x.Name).Returns("File1");

      var mockFile2 = new Mock<IFileStoreEntry>();
      mockFile2.SetupGet(x => x.Name).Returns("File2");

      var mockFileStore = new Mock<IFileStore>();
      mockFileStore.Setup(x => x.GetDirectoryContentAsync(null)).ReturnsAsync(new IFileStoreEntry[] {mockFile1.Object, mockFile2.Object});
      
      return mockFileStore.Object;
    }

  }
}
