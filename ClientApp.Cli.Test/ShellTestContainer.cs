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
  internal static class ShellTestContainer
  {
    /// <summary>
    /// The service provider for testing.
    /// </summary>
    public static IServiceProvider CreateTestContainer()
    {
      var appSettings = new AppSettings
      {
        Catalogs = new Dictionary<string, CatalogSettings>
        {
          { "default", new CatalogSettings { CatalogName = "default", MediaRoots = new Dictionary<string, string>() } }
        },
        StartupCatalog = "default"
      };

      var services = new ServiceCollection()
        .AddSingleton<ICatalogFactory>(Substitute.For<ICatalogFactory>())
        .AddSingleton<IDbConnectionSource>(Substitute.For<IDbConnectionSource>())
        .AddSingleton<AppSettings>(appSettings)
        .AddSingleton<IFileSystem>(new MockFileSystem(new Dictionary<string, MockFileData>(), @"x:\folder"))
        .AddSingleton<ICatalogTaskScanFactory>(Substitute.For<ICatalogTaskScanFactory>())
        .BuildServiceProvider();

      return services;
    }

  }
}
