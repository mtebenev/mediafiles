using McMaster.Extensions.CommandLineUtils;
using Mt.MediaFiles.ClientApp.Cli.Commands.Shell;
using Mt.MediaFiles.ClientApp.Cli.Configuration;
using Mt.MediaFiles.TestUtils;
using NSubstitute;
using System.Collections.Generic;
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

    [Fact]
    public async Task Execute_Shell_Scan_Command()
    {
      var appSettings = new AppSettings
      {
        Catalogs = new Dictionary<string, CatalogSettings>
        {
          { "default", new CatalogSettings { CatalogName = "default", MediaRoots = new Dictionary<string, string>() } }
        },
        StartupCatalog = "default"
      };

      var mockConsole = new StringConsole();
      var mockShellAppContext = Substitute.For<IShellAppContext>();
      mockShellAppContext.Console.Returns(mockConsole);
      mockShellAppContext.CatalogSettings.Returns(appSettings.Catalogs["default"]);

      var container = ShellTestContainer.CreateTestContainer();
      var app = new CommandLineApplication<CommandShell>();
      app.Conventions
        .UseDefaultConventions()
        .UseConstructorInjection(container);
      app.Model.ShellAppContext = mockShellAppContext;

      await app.ExecuteAsync(new string[] { "scan", @"x:\folder" });
    }
  }
}
