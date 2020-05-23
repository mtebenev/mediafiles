using System.Threading.Tasks;
using Xunit;
using NSubstitute;
using System.IO.Abstractions.TestingHelpers;
using Mt.MediaFiles.AppEngine.Video.Tasks;
using Mt.MediaFiles.AppEngine.Tasks;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.TestUtils;
using Mt.MediaFiles.AppEngine.Video.VideoImprint;

namespace Mt.MediaFiles.AppEngine.Video.Test.Tasks
{
  public class CatalogTaskUpdateVideoImprintsTest
  {
    [Fact]
    public async Task Should_Invoke_Updater()
    {
      var catalogDef = @"
{
  path: 'Root',
  children: [
    {
      path: 'scan_root',
      rootPath: 'x:\\root_folder',
      id: 10,
      children: [
        { id: 100, path: 'x:\\root_folder\\folder1\\video1.mp4', fileSize: 3 },
        { id: 200, path: 'x:\\root_folder\\folder1\\video2.mp4', fileSize: 3 },
        { id: 300, path: 'x:\\root_folder\\folder2\\video3.flv', fileSize: 3 },
      ]
    }
  ]
}
";
      var mockCatalog = CatalogMockBuilder.Create(catalogDef).Build();
      var mockExecutionContext = Substitute.For<ITaskExecutionContext>();
      var mockFs = new MockFileSystem();
      var mockCi = await mockCatalog.GetItemByIdAsync(10);
      var mockCatalogContext = Substitute.For<ICatalogContext>();
      mockCatalogContext.Catalog.Returns(mockCatalog);

      var mockUpdater = Substitute.For<IVideoImprintUpdater>();
      var mockUpdaterFactory = Substitute.For<IVideoImprintUpdaterFactory>();
      mockUpdaterFactory.Create().ReturnsForAnyArgs(mockUpdater);

      var task = new CatalogTaskUpdateVideoImprints(mockExecutionContext, mockFs, mockUpdaterFactory, mockCi);
      await task.ExecuteTaskAsync(mockCatalogContext);

      // Verify
      await mockUpdater.Received().UpdateAsync(Arg.Is<int>(id => id == 100), @"x:\root_folder\folder1\video1.mp4");
      await mockUpdater.Received().UpdateAsync(Arg.Is<int>(id => id == 200), @"x:\root_folder\folder1\video2.mp4");
      await mockUpdater.Received().UpdateAsync(Arg.Is<int>(id => id == 300), @"x:\root_folder\folder2\video3.flv");
    }
  }
}
