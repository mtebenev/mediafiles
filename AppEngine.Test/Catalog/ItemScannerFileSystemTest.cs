using System;
using System.Threading.Tasks;
using Moq;
using Mt.MediaMan.AppEngine.Catalog;
using Mt.MediaMan.AppEngine.CatalogStorage;
using OrchardCore.FileStorage.FileSystem;
using Xunit;

namespace Mt.MediaMan.AppEngine.Test.Catalog
{
  public class ItemScannerFileSystemTest
  {
    [Fact]
    public async Task SimpleTest()
    {
      var fileStore = new FileSystemStore(@"C:\_films");
      var mockItemStorage = new Mock<IItemStorage>();
      mockItemStorage.Setup(x => x.CreateItem(It.IsAny<CatalogItemRecord>())).Callback((CatalogItemRecord record) =>
      {
        Console.WriteLine($"Creating record for: {record.Name}");
      }).Returns(Task.FromResult(0));

      var scanner = new ItemScannerFileSystem(fileStore, mockItemStorage.Object);

      await scanner.Scan();
    }

  }
}
