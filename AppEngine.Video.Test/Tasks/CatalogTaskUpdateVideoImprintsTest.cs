using System.Threading.Tasks;
using Xunit;
using Mt.MediaMan.AppEngine.Video.Tasks;
using NSubstitute;
using Mt.MediaMan.AppEngine.Commands;
using System.IO.Abstractions.TestingHelpers;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Test.TestUtils;
using AppEngine.Video.VideoImprint;

namespace AppEngine.Video.Test.Tasks
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
      await mockUpdater.Received().UpdateAsync(Arg.Is<ICatalogItem>(x => x.CatalogItemId == 100), @"x:\root_folder\folder1\video1.mp4");
      await mockUpdater.Received().UpdateAsync(Arg.Is<ICatalogItem>(x => x.CatalogItemId == 200), @"x:\root_folder\folder1\video2.mp4");
      await mockUpdater.Received().UpdateAsync(Arg.Is<ICatalogItem>(x => x.CatalogItemId == 300), @"x:\root_folder\folder2\video3.flv");
    }
  }
}
