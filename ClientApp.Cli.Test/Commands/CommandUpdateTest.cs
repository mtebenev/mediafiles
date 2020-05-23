using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Threading.Tasks;
using Mt.MediaFiles.AppEngine.Tasks;
using Mt.MediaFiles.AppEngine.Video.Tasks;
using Mt.MediaFiles.ClientApp.Cli.Commands;
using Mt.MediaFiles.TestUtils;
using NSubstitute;
using Xunit;

namespace Mt.MediaFiles.ClientApp.Cli.Test.Commands
{
  public class CommandUpdateTest
  {
    [Fact]
    public async Task Should_Invoke_Update_Task_From_Current_Directory()
    {
      var catalogDef = @"
{
  path: 'Root',
  children: [
    {
      path: 'scan_root',
      id: 100,
      rootPath: 'x:\\root_folder',
      children: [
        { id: 11, path: 'x:\\root_folder\\file1.mp4', fileSize: 10 },
        { id: 12, path: 'x:\\root_folder\\file2.mp4', fileSize: 20 },
        { id: 13, path: 'x:\\root_folder\\file3.mp4', fileSize: 30 },
        { id: 14, path: 'x:\\root_folder\\file4.mp4', fileSize: 40 },
      ]
    }
  ]
}
";

      var mockCatalog = CatalogMockBuilder.Create(catalogDef).Build();

      var mockShellAppContext = Substitute.For<IShellAppContext>();
      mockShellAppContext.Catalog.Returns(mockCatalog);
      var rootFolderItem = await mockCatalog.GetItemByIdAsync(100);

      var mockTask = Substitute.For<CatalogTaskBase>();
      var mockFactory = Substitute.For<ICatalogTaskUpdateVideoImprintsFactory>();
      mockFactory.Create(rootFolderItem).Returns(mockTask);

      var mockFs = new MockFileSystem(null, @"x:\root_folder");

      var command = new CommandUpdate();
      var result = await command.OnExecuteAsync(mockShellAppContext, mockFs, mockFactory);

      // Verify
      await mockCatalog.Received().ExecuteTaskAsync(mockTask);
      Assert.Equal(Program.CommandExitResult, result);
    }
  }
}
