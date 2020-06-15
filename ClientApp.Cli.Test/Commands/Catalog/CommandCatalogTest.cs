using McMaster.Extensions.CommandLineUtils;
using Mt.MediaFiles.ClientApp.Cli.Commands;
using Mt.MediaFiles.ClientApp.Cli.Commands.Catalog;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace Mt.MediaFiles.ClientApp.Cli.Test.Commands.Catalog
{
  public class CommandCatalogTest
  {
    [Fact]
    public async Task Execute_Reset_Command()
    {
      var app = new CommandLineApplication<CommandCatalog>();
      app.Conventions
        .UseDefaultConventions();

      app.Model.Parent = Substitute.For<IMediaFilesApp>();

      await app.ExecuteAsync(new string[] { "reset" });
    }
  }
}
