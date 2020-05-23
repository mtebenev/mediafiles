using System.Collections.Generic;
using System.Threading.Tasks;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using Mt.MediaFiles.AppEngine.Scanning;
using Mt.MediaFiles.AppEngine.Tasks;
using Mt.MediaFiles.ClientApp.Cli.Commands.Shell;
using Mt.MediaFiles.TestUtils;
using NSubstitute;
using Xunit;

namespace Mt.MediaFiles.ClientApp.Cli.Test.Commands
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

      var command = new CommandShellScan();
      command.PathAlias = "some_media_root";
      command.Name = "given_root_name";

      await command.OnExecuteAsync(mockShellAppContext, mockCatalogSettings, mockTaskFactory);

      mockTaskFactory.Received().Create(Arg.Is(
        (ScanParameters p) =>
        p.ScanPath == "some_media_root_path"
        && p.RootItemName == "given_root_name"
        )
      );
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

      var command = new CommandShellScan();
      command.PathAlias = "some_fs_path";
      command.Name = "given_root_name";

      await command.OnExecuteAsync(mockShellAppContext, mockCatalogSettings, mockTaskFactory);
      mockTaskFactory.Received().Create(Arg.Is(
        (ScanParameters p) =>
        p.ScanPath == "some_fs_path"
        && p.RootItemName == "given_root_name"
        )
      );
    }
  }
}
