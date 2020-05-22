using Mt.MediaFiles.ClientApp.Cli.Commands.Catalog;
using Mt.MediaFiles.ClientApp.Cli.Configuration;
using Mt.MediaFiles.TestUtils;
using NSubstitute;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Mt.MediaFiles.ClientApp.Cli.Test.Commands.Catalog
{
  public class CommandCatalogUseTest
  {
    [Fact]
    public async Task Should_Switch_Existing_Catalog()
    {
      var configJson = @"{
  'startupCatalog': 'catalog1',
  'catalogs': {
    'catalog1': {
      'catalogName': 'catalog1'
    },
    'catalog2': {
      'catalogName': 'catalog2'
    }
  }
}";
      var appSettings = JsonSerializer.Deserialize<AppSettings>(configJson.Replace('\'', '\"'), new JsonSerializerOptions { PropertyNamingPolicy =  JsonNamingPolicy.CamelCase });
      var mockConsole = new StringConsole();
      var mockAppSettingsManager = Substitute.For<IAppSettingsManager>();

      var shellAppContext = new ShellAppContext(mockAppSettingsManager);
      var command = new CommandCatalogUse();
      command.CatalogName = "catalog2";

      await command.OnExecute(appSettings, mockConsole, shellAppContext);

      Assert.Equal("catalog2", appSettings.StartupCatalog);
      mockAppSettingsManager.Received().Update();
    }
  }
}
