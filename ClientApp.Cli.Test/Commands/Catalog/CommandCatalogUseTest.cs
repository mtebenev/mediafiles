using Mt.MediaFiles.ClientApp.Cli.Commands.Catalog;
using Mt.MediaFiles.ClientApp.Cli.Configuration;
using Mt.MediaFiles.TestUtils;
using NSubstitute;
using System;
using System.IO.Abstractions;
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
      var appSettings = JsonSerializer.Deserialize<AppSettings>(configJson.Replace('\'', '\"'), new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
      var mockConsole = new StringConsole();
      var mockAppSettingsManager = Substitute.For<IAppSettingsManager>();
      var mockEnvironment = Substitute.For<IEnvironment>();
      var mockFs = Substitute.For<IFileSystem>();

      var shellAppContext = new ShellAppContext(mockAppSettingsManager);
      var command = new CommandCatalogUse();
      command.CatalogName = "catalog2";

      await command.OnExecute(appSettings, mockConsole, shellAppContext, mockEnvironment, mockFs);

      Assert.Equal("catalog2", appSettings.StartupCatalog);
      mockAppSettingsManager.Received().Update();
    }

    [Fact]
    public async Task Should_Throw_When_Unknown_Catalog()
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
      var appSettings = JsonSerializer.Deserialize<AppSettings>(configJson.Replace('\'', '\"'), new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
      var mockConsole = new StringConsole();
      var mockAppSettingsManager = Substitute.For<IAppSettingsManager>();
      var mockEnvironment = Substitute.For<IEnvironment>();
      var mockFs = Substitute.For<IFileSystem>();

      var shellAppContext = new ShellAppContext(mockAppSettingsManager);
      var command = new CommandCatalogUse();
      command.CatalogName = "unknown-catalog";

      await Assert.ThrowsAsync<InvalidOperationException>(async () =>
      {
        await command.OnExecute(appSettings, mockConsole, shellAppContext, mockEnvironment, mockFs);
      });

      mockAppSettingsManager.DidNotReceive().Update();
    }

    [Fact]
    public async Task Should_Create_New_Catalog()
    {
      var configJson = @"{
  'startupCatalog': 'catalog1',
  'catalogs': {
    'catalog1': {
      'catalogName': 'catalog1'
    }
  }
}";
      var appSettings = JsonSerializer.Deserialize<AppSettings>(configJson.Replace('\'', '\"'), new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
      var mockConsole = new StringConsole();
      var mockAppSettingsManager = Substitute.For<IAppSettingsManager>();
      var mockEnvironment = Substitute.For<IEnvironment>();
      var mockFs = Substitute.For<IFileSystem>();

      var shellAppContext = new ShellAppContext(mockAppSettingsManager);
      var command = new CommandCatalogUse();
      command.CatalogName = "new-catalog";
      command.Create = true;

      await command.OnExecute(appSettings, mockConsole, shellAppContext, mockEnvironment, mockFs);

      Assert.Equal("new-catalog", appSettings.StartupCatalog);
      Assert.True(appSettings.Catalogs.ContainsKey("new-catalog"));

      var catalogSettings = appSettings.Catalogs["new-catalog"];
      Assert.False(string.IsNullOrEmpty(catalogSettings.ConnectionString));
      Assert.Empty(catalogSettings.MediaRoots);

      mockAppSettingsManager.Received().Update();
    }
  }
}
