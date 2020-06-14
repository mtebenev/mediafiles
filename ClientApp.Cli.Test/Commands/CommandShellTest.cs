using McMaster.Extensions.CommandLineUtils;
using Mt.MediaFiles.ClientApp.Cli.Commands.Shell;
using Mt.MediaFiles.TestUtils;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace Mt.MediaFiles.ClientApp.Cli.Test.Commands
{
  public class CommandShellTest
  {
    [Fact]
    public async Task Execute_Simple_Shell_Commands()
    {
      var mockConsole = new StringConsole();
      var mockShellAppContext = Substitute.For<IShellAppContext>();
      mockShellAppContext.Console.Returns(mockConsole);

      var app = new CommandLineApplication<CommandShell>();
      app.Conventions
        .UseDefaultConventions();
      app.Model.ShellAppContext = mockShellAppContext;

      await app.ExecuteAsync(new string[] { "ls" });
      await app.ExecuteAsync(new string[] { "cd", ":1" });
    }
  }
}
