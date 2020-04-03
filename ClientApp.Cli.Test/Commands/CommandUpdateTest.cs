using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Tasks;
using Mt.MediaMan.AppEngine.Test.TestUtils;
using Mt.MediaMan.AppEngine.Video.Tasks;
using Mt.MediaMan.ClientApp.Cli;
using Mt.MediaMan.ClientApp.Cli.Commands;
using NSubstitute;
using Xunit;

namespace ClientApp.Cli.Test.Commands
{
  internal class CommandUpdateTesing : CommandUpdate
  {
    public CommandUpdateTesing(
      IShellAppContext shellAppContext,
      IFileSystem fileSystem,
      ICatalogTaskUpdateVideoImprintsFactory updateVideoImprintsFactory
      ) : base(shellAppContext, fileSystem, updateVideoImprintsFactory)
    {
    }

    public Task<int> ExecuteAsync()
    {
      return this.OnExecuteAsync();
    }
  }

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
      
      var command = new CommandUpdateTesing(mockShellAppContext, mockFs, mockFactory);
      var result = await command.ExecuteAsync();

      // Verify
      await mockCatalog.Received().ExecuteTaskAsync(mockTask);
      Assert.Equal(Program.CommandExitResult, result);
    }
  }
}
