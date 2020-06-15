using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Threading.Tasks;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using Mt.MediaFiles.AppEngine.Scanning;
using Mt.MediaFiles.AppEngine.Tasks;
using Mt.MediaFiles.ClientApp.Cli.Commands;
using Mt.MediaFiles.TestUtils;
using NSubstitute;
using Xunit;

namespace ClientApp.Cli.Test.Commands
{
  public class CommandScanTest
  {
    [Fact]
    public async Task Should_Use_Current_Directory_By_Default()
    {
      var mockCatalogSettings = Substitute.For<ICatalogSettings>();
      mockCatalogSettings.MediaRoots.Returns(new Dictionary<string, string>());

      var mockFs = new MockFileSystem(null, @"x:\root_folder");
      var mockTaskFactory = Substitute.For<ICatalogTaskScanFactory>();

      var mockMfApp = Substitute.For<IMediaFilesApp>();

      var command = new CommandScan();
      command.Parent = mockMfApp;
      await command.OnExecuteAsync(new StringConsole(), mockFs, mockTaskFactory);

      // Verify
      mockTaskFactory.Received().Create(Arg.Is<ScanParameters>(x => x.ScanPath == @"x:\root_folder"));
    }

    [Fact]
    public async Task Should_Resolve_Relative_Path()
    {
      var mockCatalogSettings = Substitute.For<ICatalogSettings>();
      mockCatalogSettings.MediaRoots.Returns(new Dictionary<string, string>());

      var mockFs = new MockFileSystem(
        new Dictionary<string, MockFileData>
        {
          { @"x:\root_folder\folder1\file1.txt", new MockFileData("abc") },
        },
        @"x:\root_folder"
      );
      var mockTaskFactory = Substitute.For<ICatalogTaskScanFactory>();
      var mockMfApp = Substitute.For<IMediaFilesApp>();
      mockMfApp.GetCatalogSettings().Returns(mockCatalogSettings);

      var command = new CommandScan();
      command.Parent = mockMfApp;
      command.PathAlias = "folder1";

      await command.OnExecuteAsync(new StringConsole(), mockFs, mockTaskFactory);

      // Verify
      mockTaskFactory.Received().Create(Arg.Is<ScanParameters>(x => x.ScanPath == @"x:\root_folder\folder1"));
    }

    [Fact]
    public async Task Should_Resolve_Absolute_Path()
    {
      var mockCatalogSettings = Substitute.For<ICatalogSettings>();
      mockCatalogSettings.MediaRoots.Returns(new Dictionary<string, string>());

      var mockFs = new MockFileSystem(
        new Dictionary<string, MockFileData>
        {
          { @"x:\root_folder\folder1\file1.txt", new MockFileData("abc") },
        },
        @"x:\some_other_folder"
      );
      var mockTaskFactory = Substitute.For<ICatalogTaskScanFactory>();
      var mockMfApp = Substitute.For<IMediaFilesApp>();
      mockMfApp.GetCatalogSettings().Returns(mockCatalogSettings);

      var command = new CommandScan();
      command.Parent = mockMfApp;
      command.PathAlias = @"x:\root_folder\folder1";

      await command.OnExecuteAsync(new StringConsole(), mockFs, mockTaskFactory);

      // Verify
      mockTaskFactory.Received().Create(Arg.Is<ScanParameters>(x => x.ScanPath == @"x:\root_folder\folder1"));
    }

    [Fact]
    public async Task Should_Resolve_Media_Roots()
    {
      var mockCatalogSettings = Substitute.For<ICatalogSettings>();
      mockCatalogSettings.MediaRoots.Returns(new Dictionary<string, string>
      {
        {"media_root1", "some_path" },
        {"some_media_root", @"x:\root_folder\folder1" }
      });
      var mockFs = new MockFileSystem(
        new Dictionary<string, MockFileData>
        {
          { @"x:\root_folder\folder1\file1.txt", new MockFileData("abc") },
        },
        @"x:\some_other_folder"
      );

      var mockTaskFactory = Substitute.For<ICatalogTaskScanFactory>();
      var mockMfApp = Substitute.For<IMediaFilesApp>();
      mockMfApp.GetCatalogSettings().Returns(mockCatalogSettings);

      var command = new CommandScan();
      command.Parent = mockMfApp;
      command.PathAlias = "some_media_root";
      command.Name = "given_root_name";

      await command.OnExecuteAsync(new StringConsole(), mockFs, mockTaskFactory);

      mockTaskFactory.Received()
        .Create(Arg.Is<ScanParameters>(x => x.ScanPath == @"x:\root_folder\folder1"));
    }
  }
}
