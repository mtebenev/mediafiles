using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using Mt.MediaFiles.AppEngine.Matching;
using Mt.MediaFiles.AppEngine.Tasks;
using Mt.MediaFiles.AppEngine.Video.Tasks;
using Mt.MediaFiles.ClientApp.Cli.Commands;
using Mt.MediaFiles.ClientApp.Cli.Configuration;
using Mt.MediaFiles.ClientApp.Cli.Core;
using Mt.MediaFiles.TestUtils;
using NSubstitute;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Mt.MediaFiles.ClientApp.Cli.Test
{
  public class CommandMediaFilesTest
  {
    [Fact]
    public async Task Should_Open_Default_Catalog()
    {
      var serviceProvder = new ServiceCollection()
        .AddSingleton<IStorageManager>(Substitute.For<IStorageManager>())
        .BuildServiceProvider();

      var mockConsole = new StringConsole();
      var mockCatalogFactory = Substitute.For<ICatalogFactory>();
      var dbConnProvider = Substitute.For<IDbConnectionSource>();
      var appSettings = new AppSettings
      {
        Catalogs = new Dictionary<string, CatalogSettings>
        {
          { "default", new CatalogSettings { CatalogName = "name-default", ConnectionString = "connstr-default" } },
          { "context", new CatalogSettings { CatalogName = "name-context", ConnectionString = "connstr-context" } },
        },
        StartupCatalog = "default"
      };

      var command = new CommandMediaFiles(serviceProvder, mockConsole, mockCatalogFactory, dbConnProvider, appSettings);
      await command.OpenCatalogAsync();

      dbConnProvider.Received().SetConnectionString("connstr-default");
    }

    [Fact]
    public async Task Should_Open_Context_Catalog()
    {
      var serviceProvder = new ServiceCollection()
        .AddSingleton<IStorageManager>(Substitute.For<IStorageManager>())
        .BuildServiceProvider();

      var mockConsole = new StringConsole();
      var mockCatalogFactory = Substitute.For<ICatalogFactory>();
      var dbConnProvider = Substitute.For<IDbConnectionSource>();
      var appSettings = new AppSettings
      {
        Catalogs = new Dictionary<string, CatalogSettings>
        {
          { "default", new CatalogSettings { CatalogName = "name-default", ConnectionString = "connstr-default" } },
          { "context", new CatalogSettings { CatalogName = "name-context", ConnectionString = "connstr-context" } },
        },
        StartupCatalog = "default"
      };

      var command = new CommandMediaFiles(serviceProvder, mockConsole, mockCatalogFactory, dbConnProvider, appSettings);
      command.CatalogName = "context";
      await command.OpenCatalogAsync();

      dbConnProvider.Received().SetConnectionString("connstr-context");
    }

    [Fact]
    public async Task Execute_Scan_Command()
    {
      var app = new CommandLineApplication<CommandMediaFiles>();
      var services = TestContainerBuilder
        .Create()
        .AddSingletonMock<ICatalogTaskScanFactory>()
        .Build();

      app.Conventions
        .UseDefaultConventions()
        .UseConstructorInjection(services);

      await app.ExecuteAsync(new string[] { "scan", @"x:\folder" });
    }

    [Fact]
    public async Task Execute_Search_Vdups_Command()
    {
      var app = new CommandLineApplication<CommandMediaFiles>();
      var services = TestContainerBuilder
        .Create()
        .AddSingletonMock<ICatalogTaskSearchVideoDuplicatesFactory>()
        .AddCatalogTaskResult(new MatchResult(new List<MatchResultGroup>()))
        .Build();

      app.Conventions
        .UseDefaultConventions()
        .UseConstructorInjection(services);

      await app.ExecuteAsync(new string[] { "search-vdups" });
    }
  }
}
