using Microsoft.Extensions.DependencyInjection;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.Tasks;
using Mt.MediaFiles.ClientApp.Cli.Configuration;
using Mt.MediaFiles.ClientApp.Cli.Core;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;

namespace Mt.MediaFiles.ClientApp.Cli.Test
{
  /// <summary>
  /// Helpers for creating container full of mocks for testing.
  /// </summary>
  internal class TestContainerBuilder
  {
    private readonly ServiceCollection _serviceCollection;
    private readonly ICatalog _mockCatalog;

    /// <summary>
    /// Factory method.
    /// </summary>
    public static TestContainerBuilder Create()
    {
      return new TestContainerBuilder();
    }

    private TestContainerBuilder()
    {
      this._serviceCollection = new ServiceCollection();
      this._mockCatalog = Substitute.For<ICatalog>();
    }

    /// <summary>
    /// The catalog mock instance.
    /// </summary>
    public ICatalog MockCatalog => this._mockCatalog;

    /// <summary>
    /// Add a singleton to the test container.
    /// </summary>
    public TestContainerBuilder AddSingleton<TService>(TService instance) where TService: class
    {
      this._serviceCollection.AddSingleton<TService>(instance);
      return this;
    }

    /// <summary>
    /// Add an empty mock singleton.
    /// </summary>
    public TestContainerBuilder AddSingletonMock<TService>() where TService : class
    {
      var mockService = Substitute.For<TService>();
      this._serviceCollection.AddSingleton<TService>(mockService);
      return this;
    }

    /// <summary>
    /// Adds mock for catalog task execution returning specified result.
    /// </summary>
    public TestContainerBuilder AddCatalogTaskResult<TResult>(TResult resultValue)
    {
      this._mockCatalog
        .ExecuteTaskAsync(Arg.Any<CatalogTaskBase<TResult>>())
        .ReturnsForAnyArgs(resultValue);

      return this;
    }

    /// <summary>
    /// The final call..
    /// </summary>
    public IServiceProvider Build()
    {
      var appSettings = new AppSettings
      {
        Catalogs = new Dictionary<string, CatalogSettings>
        {
          { "default", new CatalogSettings { CatalogName = "default", MediaRoots = new Dictionary<string, string>() } }
        },
        StartupCatalog = "default"
      };

      var mockAppSettingsManager = Substitute.For<IAppSettingsManager>();
      mockAppSettingsManager.AppSettings.Returns(appSettings);

      var mockCatalogFactory = Substitute.For<ICatalogFactory>();
      mockCatalogFactory.OpenCatalogAsync(default, default).ReturnsForAnyArgs(this._mockCatalog);

      var services = this._serviceCollection
        .AddSingleton<ICatalogFactory>(mockCatalogFactory)
        .AddSingleton<IDbConnectionSource>(Substitute.For<IDbConnectionSource>())
        .AddSingleton<AppSettings>(appSettings)
        .AddSingleton<IAppSettingsManager>(mockAppSettingsManager)
        .AddSingleton<IEnvironment>(Substitute.For<IEnvironment>())
        .AddSingleton<IFileSystem>(new MockFileSystem(new Dictionary<string, MockFileData>(), @"x:\folder"))
        .BuildServiceProvider();

      return services;
    }
  }
}
