using Mt.MediaFiles.ClientApp.Cli.Configuration;
using NSubstitute;
using System.IO;
using System.IO.Abstractions;
using System.Text;
using Xunit;

namespace Mt.MediaFiles.ClientApp.Cli.Test.Configuration
{
  public class AppSettingsManagerTest
  {
    [Fact]
    public void Should_Create_Default_Settings()
    {
      var mockEnvironment = Substitute.For<IEnvironment>();
      mockEnvironment.GetDataPath().Returns(@"x:\data_path");

      var mockFs = Substitute.For<IFileSystem>();
      mockFs.Path.Combine(default, default).ReturnsForAnyArgs(x => Path.Combine((string)x[0], (string)x[1]));

      var manager = AppSettingsManager.Create(mockEnvironment, mockFs);

      // Verify
      Assert.Equal(@"x:\data_path\.mediafiles", manager.AppEngineSettings.DataDirectory);
      Assert.False(manager.AppSettings.ExperimentalMode);
      Assert.Equal("default", manager.AppSettings.StartupCatalog);
      Assert.Single(manager.AppSettings.Catalogs);
      Assert.Equal("default", manager.AppSettings.Catalogs["default"].CatalogName);
      mockFs.File.Received().WriteAllText(@"x:\data_path\.mediafiles\appsettings.json", Arg.Any<string>()); // Should save the new configuration
    }

    [Fact]
    public void Should_Read_Config()
    {
      var configJson = @"{
  'startupCatalog': 'local',
  'catalogs': {
    'local': {
      'catalogName': 'local'
    }
  }
}";

      var mockEnvironment = Substitute.For<IEnvironment>();
      mockEnvironment.GetDataPath().Returns(@"x:\data_path");

      var mockFs = Substitute.For<IFileSystem>();
      mockFs.Path.Combine(default, default).ReturnsForAnyArgs(x => Path.Combine((string)x[0], (string)x[1]));
      mockFs.File.Exists(@"x:\data_path\.mediafiles\appsettings.json").Returns(true);
      mockFs.File.OpenRead(@"x:\data_path\.mediafiles\appsettings.json")
        .Returns(new MemoryStream(Encoding.UTF8.GetBytes(configJson.Replace('\'', '\"'))));

      var manager = AppSettingsManager.Create(mockEnvironment, mockFs);

      // Verify
      Assert.Equal("local", manager.AppSettings.StartupCatalog);
      Assert.Single(manager.AppSettings.Catalogs);
      Assert.Equal("local", manager.AppSettings.Catalogs["local"].CatalogName);
      mockFs.File.DidNotReceiveWithAnyArgs().WriteAllText(Arg.Any<string>(), Arg.Any<string>());
    }

    [Fact]
    public void Should_Use_Environment_Config()
    {
      var configJsonDefault = @"{
  'startupCatalog': 'local',
  'catalogs': {
    'local': {
      'catalogName': 'local'
    }
  }
}";
      var configJsonDev = @"{
  'startupCatalog': 'dev',
  'catalogs': {
    'dev': {
      'catalogName': 'dev'
    }
  }
}";

      var mockEnvironment = Substitute.For<IEnvironment>();
      mockEnvironment.GetDataPath().Returns(@"x:\data_path");
      mockEnvironment.GetBaseDirectory().Returns(@"x:\app_path");
      mockEnvironment.GetEnvironmentVariable("MF_ENVIRONMENT").Returns("dev");

      var mockFs = Substitute.For<IFileSystem>();
      mockFs.Path.Combine(default, default).ReturnsForAnyArgs(x => Path.Combine((string)x[0], (string)x[1]));

      mockFs.File.Exists(@"x:\data_path\.mediafiles\appsettings.json").Returns(true);
      mockFs.File.Exists(@"x:\data_path\.mediafiles\appsettings.dev.json").Returns(true);

      mockFs.File.OpenRead(@"x:\data_path\.mediafiles\appsettings.json")
        .Returns(new MemoryStream(Encoding.UTF8.GetBytes(configJsonDefault.Replace('\'', '\"'))));
      mockFs.File.OpenRead(@"x:\data_path\.mediafiles\appsettings.dev.json")
        .Returns(new MemoryStream(Encoding.UTF8.GetBytes(configJsonDev.Replace('\'', '\"'))));

      var manager = AppSettingsManager.Create(mockEnvironment, mockFs);

      // Verify
      Assert.Equal("dev", manager.AppSettings.StartupCatalog);
      Assert.Equal(2, manager.AppSettings.Catalogs.Count);
      Assert.Equal("dev", manager.AppSettings.Catalogs["dev"].CatalogName);
      mockFs.File.DidNotReceiveWithAnyArgs().WriteAllText(Arg.Any<string>(), Arg.Any<string>());
    }

    [Fact]
    public void Should_Use_Exe_Local_Config()
    {
      var configJsonDefault = @"{
  'startupCatalog': 'local',
  'catalogs': {
    'local': {
      'catalogName': 'local'
    }
  }
}";
      var configJsonExe = @"{
  'startupCatalog': 'exe',
  'catalogs': {
    'exe': {
      'catalogName': 'exe'
    }
  }
}";

      var mockEnvironment = Substitute.For<IEnvironment>();
      mockEnvironment.GetDataPath().Returns(@"x:\data_path");
      mockEnvironment.GetBaseDirectory().Returns(@"x:\app_path");
      mockEnvironment.GetEnvironmentVariable("MF_ENVIRONMENT").Returns("dev");

      var mockFs = Substitute.For<IFileSystem>();
      mockFs.Path.Combine(default, default).ReturnsForAnyArgs(x => Path.Combine((string)x[0], (string)x[1]));

      mockFs.File.Exists(@"x:\data_path\.mediafiles\appsettings.json").Returns(true);
      mockFs.File.Exists(@"x:\app_path\appsettings.json").Returns(true);

      mockFs.File.OpenRead(@"x:\data_path\.mediafiles\appsettings.json")
        .Returns(new MemoryStream(Encoding.UTF8.GetBytes(configJsonDefault.Replace('\'', '\"'))));
      mockFs.File.OpenRead(@"x:\app_path\appsettings.json")
        .Returns(new MemoryStream(Encoding.UTF8.GetBytes(configJsonExe.Replace('\'', '\"'))));

      var manager = AppSettingsManager.Create(mockEnvironment, mockFs);

      // Verify
      Assert.Equal("exe", manager.AppSettings.StartupCatalog);
      Assert.Single(manager.AppSettings.Catalogs);
      Assert.Equal("exe", manager.AppSettings.Catalogs["exe"].CatalogName);
      mockFs.File.DidNotReceiveWithAnyArgs().WriteAllText(Arg.Any<string>(), Arg.Any<string>());
    }
  }
}
