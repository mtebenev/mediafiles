using Mt.MediaFiles.ClientApp.Cli.Commands.Catalog;
using Mt.MediaFiles.ClientApp.Cli.Configuration;
using Mt.MediaFiles.TestUtils;
using NSubstitute;
using System;
using System.IO.Abstractions;
using System.Text.Json;
using Xunit;

namespace Mt.MediaFiles.ClientApp.Cli.Test.Commands.Catalog
{
  public class CommandCatalogUseTest
  {
    [Fact]
    public void Should_Switch_Existing_Catalog()
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
      var mockAppSettingsManager = Substitute.For<IAppSettingsManager>();
      mockAppSettingsManager.AppSettings.Returns(appSettings);
      var mockEnvironment = Substitute.For<IEnvironment>();
      var mockFs = Substitute.For<IFileSystem>();

      var command = new CommandCatalogUse();
      command.CatalogName = "catalog2";

      command.OnExecute(
        mockAppSettingsManager,
        new StringConsole(),
        mockEnvironment,
        mockFs);

      Assert.Equal("catalog2", appSettings.StartupCatalog);
      mockAppSettingsManager.Received().Update();
    }

    [Fact]
    public void Should_Throw_When_Unknown_Catalog()
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
      var mockAppSettingsManager = Substitute.For<IAppSettingsManager>();
      mockAppSettingsManager.AppSettings.Returns(appSettings);
      var mockEnvironment = Substitute.For<IEnvironment>();
      var mockFs = Substitute.For<IFileSystem>();

      var command = new CommandCatalogUse();
      command.CatalogName = "unknown-catalog";

      Assert.Throws<InvalidOperationException>(() =>
      {
        command.OnExecute(
          mockAppSettingsManager,
          new StringConsole(),
          mockEnvironment,
          mockFs);
      });

      mockAppSettingsManager.DidNotReceive().Update();
    }

    [Fact]
    public void Should_Create_New_Catalog()
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
      var mockAppSettingsManager = Substitute.For<IAppSettingsManager>();
      mockAppSettingsManager.AppSettings.Returns(appSettings);
      var mockEnvironment = Substitute.For<IEnvironment>();
      var mockFs = Substitute.For<IFileSystem>();

      var command = new CommandCatalogUse();
      command.CatalogName = "new-catalog";
      command.Create = true;

      command.OnExecute(
        mockAppSettingsManager,
        new StringConsole(),
        mockEnvironment,
        mockFs);

      Assert.Equal("new-catalog", appSettings.StartupCatalog);
      Assert.True(appSettings.Catalogs.ContainsKey("new-catalog"));

      var catalogSettings = appSettings.Catalogs["new-catalog"];
      Assert.False(string.IsNullOrEmpty(catalogSettings.ConnectionString));
      Assert.Empty(catalogSettings.MediaRoots);

      mockAppSettingsManager.Received().Update();
    }

    [Fact]
    public void Should_Print_Startup_Catalog()
    {
      var configJson = @"{
  'startupCatalog': 'catalog2',
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
      var mockAppSettingsManager = Substitute.For<IAppSettingsManager>();
      mockAppSettingsManager.AppSettings.Returns(appSettings);
      var mockEnvironment = Substitute.For<IEnvironment>();
      var mockFs = Substitute.For<IFileSystem>();
      var mockConsole = new StringConsole();

      var command = new CommandCatalogUse();

      command.OnExecute(
        mockAppSettingsManager,
        mockConsole,
        mockEnvironment,
        mockFs);

      mockAppSettingsManager.DidNotReceive().Update();
      Assert.Equal("Startup catalog: catalog2\r\n", mockConsole.GetText());
    }
  }
}
