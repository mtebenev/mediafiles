using System.Collections.Generic;
using System.IO.Abstractions;
using FluentAssertions;
using Mt.MediaFiles.ClientApp.Cli.Configuration;
using NSubstitute;
using Xunit;

namespace Mt.MediaFiles.ClientApp.Cli.Test.Configuration
{
  public class DefaultSettingsTest
  {
    [Fact]
    public void Keep_Original_Settings_If_Valid()
    {
      var mockEnvironment = Substitute.For<IEnvironment>();
      var mockFs = Substitute.For<IFileSystem>();

      var appSettings = new AppSettings
      {
        Catalogs = new Dictionary<string, CatalogSettings>
        {
          {"some-catalog", new CatalogSettings {CatalogName = "some-catalog", ConnectionString = "conn", MediaRoots = null} }
        },
        StartupCatalog = "some-catalog"
      };

      var resultSettings = DefaultSettings.FillDefaultAppSettings(appSettings, mockEnvironment, mockFs);
      resultSettings.Should().BeEquivalentTo(appSettings);
    }

    [Fact]
    public void Create_Default_Settings()
    {
      var mockEnvironment = Substitute.For<IEnvironment>();
      mockEnvironment.GetDataPath().Returns("user-data-path");

      var mockFs = Substitute.For<IFileSystem>();
      const string defaultDataSource = @"user-data-path\.mediafiles\default.db";
      mockFs.Path.Combine("user-data-path", ".mediafiles").Returns(@"user-data-path\.mediafiles");
      mockFs.Path.Combine(@"user-data-path\.mediafiles", "default.db").Returns(defaultDataSource);

      var resultSettings = DefaultSettings.FillDefaultAppSettings(null, mockEnvironment, mockFs);

      resultSettings.Should().BeEquivalentTo(
        new AppSettings
        {
          StartupCatalog = "default",
          Catalogs = new Dictionary<string, CatalogSettings>
          {
            { "default", new CatalogSettings
              {
                CatalogName = "default",
                MediaRoots = new Dictionary<string, string>(),
                ConnectionString = $"Data Source={defaultDataSource}"
              }
            }
          }
        }
      );
    }

    [Fact]
    public void Should_Create_Default_Catalog()
    {
      var mockEnvironment = Substitute.For<IEnvironment>();
      mockEnvironment.GetDataPath().Returns("user-data-path");

      var mockFs = Substitute.For<IFileSystem>();
      const string defaultDataSource = @"user-data-path\.mediafiles\default.db";
      mockFs.Path.Combine("user-data-path", ".mediafiles").Returns(@"user-data-path\.mediafiles");
      mockFs.Path.Combine(@"user-data-path\.mediafiles", "default.db").Returns(defaultDataSource);

      var resultSettings = DefaultSettings.FillDefaultAppSettings(new AppSettings(), mockEnvironment, mockFs);

      resultSettings.Should().BeEquivalentTo(
        new AppSettings
        {
          StartupCatalog = "default",
          Catalogs = new Dictionary<string, CatalogSettings>
          {
            { "default", new CatalogSettings
              {
                CatalogName = "default",
                MediaRoots = new Dictionary<string, string>(),
                ConnectionString = $"Data Source={defaultDataSource}"
              }
            }
          }
        }
      );
    }

    [Fact]
    public void Should_Fill_Startup_Catalog()
    {
      var mockEnvironment = Substitute.For<IEnvironment>();
      var mockFs = Substitute.For<IFileSystem>();

      var settings = new AppSettings
      {
        Catalogs = new Dictionary<string, CatalogSettings>
        {
          { "catalog1", new CatalogSettings() },
          { "catalog2", new CatalogSettings() },
        }
      };

      var resultSettings = DefaultSettings.FillDefaultAppSettings(settings, mockEnvironment, mockFs);

      Assert.Equal("catalog1", resultSettings.StartupCatalog);
    }

    [Fact]
    public void Should_Fill_Startup_Catalog_If_Invalid()
    {
      var mockEnvironment = Substitute.For<IEnvironment>();
      var mockFs = Substitute.For<IFileSystem>();

      var settings = new AppSettings
      {
        StartupCatalog = "some-other-catalog",
        Catalogs = new Dictionary<string, CatalogSettings>
        {
          { "catalog1", new CatalogSettings() },
          { "catalog2", new CatalogSettings() },
        }
      };

      var resultSettings = DefaultSettings.FillDefaultAppSettings(settings, mockEnvironment, mockFs);

      Assert.Equal("catalog1", resultSettings.StartupCatalog);
    }
  }
}
