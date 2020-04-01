using System.Collections.Generic;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.CatalogStorage;
using Mt.MediaMan.AppEngine.Tasks;
using Mt.MediaMan.AppEngine.Test.TestUtils;
using Mt.MediaMan.ClientApp.Cli;
using Mt.MediaMan.ClientApp.Cli.Commands;
using NSubstitute;
using Xunit;

namespace ClientApp.Cli.Test.Commands
{
  public class ShellCommandScanTest
  {
    [Fact]
    public async Task Should_Scan_Media_Root()
    {
      var mockShellAppContext = Substitute.For<IShellAppContext>();
      mockShellAppContext.Console.Returns(new StringConsole());
      var mockCatalogSettings = Substitute.For<ICatalogSettings>();
      mockCatalogSettings.MediaRoots.Returns(new Dictionary<string, string>
      {
        {"media_root1", "some_path" },
        {"some_media_root", "some_media_root_path" }
      });
      
      var mockTaskFactory = Substitute.For<ICatalogTaskScanFactory>();

      var task = new ShellCommandScan();
      task.PathAlias = "some_media_root";
      task.Name = "given_root_name";

      await task.OnExecuteAsync(mockShellAppContext, mockCatalogSettings, mockTaskFactory);

      mockTaskFactory.Received().Create("some_media_root_path", "given_root_name");
    }

    [Fact]
    public async Task Should_Use_Fs_Path()
    {
      var mockShellAppContext = Substitute.For<IShellAppContext>();
      mockShellAppContext.Console.Returns(new StringConsole());
      var mockCatalogSettings = Substitute.For<ICatalogSettings>();
      mockCatalogSettings.MediaRoots.Returns(new Dictionary<string, string>
      {
      });

      var mockTaskFactory = Substitute.For<ICatalogTaskScanFactory>();

      var task = new ShellCommandScan();
      task.PathAlias = "some_fs_path";
      task.Name = "given_root_name";

      await task.OnExecuteAsync(mockShellAppContext, mockCatalogSettings, mockTaskFactory);

      mockTaskFactory.Received().Create("some_fs_path", "given_root_name");
    }
  }
}
