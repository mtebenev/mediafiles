using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Threading.Tasks;
using FluentAssertions;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaFiles.TestUtils;
using Mt.MediaMan.AppEngine.Commands;
using Mt.MediaMan.AppEngine.Test.TestUtils;
using Mt.MediaMan.ClientApp.Cli;
using Mt.MediaMan.ClientApp.Cli.Commands;
using NSubstitute;
using Xunit;

namespace ClientApp.Cli.Test.Commands
{
  internal class CommandCheckStatusTesing : CommandCheckStatus
  {
    public Task<int> ExecuteAsync(IShellAppContext shellAppContext, IFileSystem fileSystem, ICatalogTaskCheckStatusFactory taskFactory, IConsole console)
    {
      return this.OnExecuteAsync(shellAppContext, fileSystem, taskFactory, console);
    }
  }

  public class CommandCheckStatusTest
  {
    [Fact]
    public async Task Should_Invoke_Task()
    {
      var catalogDef = @"
{
  path: 'Root',
  children: [
    {
      path: 'scan_root',
      id: 100,
      rootPath: 'x:\\root_folder'
    }
  ]
}
";
      var mockCatalog = CatalogMockBuilder.Create(catalogDef).Build();
      var mockShellAppContext = Substitute.For<IShellAppContext>();
      mockShellAppContext.Catalog.Returns(mockCatalog);

      var mockTaskFactory = Substitute.For<ICatalogTaskCheckStatusFactory>();
      var console = new StringConsole();

      mockCatalog.ExecuteTaskAsync<IList<CheckStatusResult>>(default).ReturnsForAnyArgs(new[]
      {
        new CheckStatusResult {CatalogItemId = 10, Path = "path_1", Status = FsItemStatus.Ok },
        new CheckStatusResult {CatalogItemId = 20, Path = "path_2", Status = FsItemStatus.Ok },
        new CheckStatusResult {CatalogItemId = 30, Path = "path_3", Status = FsItemStatus.Ok },
      });

      var mockFs = new MockFileSystem(null, @"x:\root_folder");

      var command = new CommandCheckStatusTesing();
      var result = await command.ExecuteAsync(mockShellAppContext, mockFs, mockTaskFactory, console);

      var output = console.GetText();
      output.Should().ContainEquivalentOf(
        "10  path_1  Ok",
        "20  path_2  Ok",
        "30  path_3  Ok"
        );

      console.Dispose();
      Assert.Equal(Program.CommandExitResult, result);
    }
  }
}
