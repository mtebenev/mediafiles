using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using Microsoft.Data.Sqlite;
using Mt.MediaFiles.AppEngine;

namespace Mt.MediaFiles.ClientApp.Cli.Configuration
{
  /// <summary>
  /// Utility class filling the default settings.
  /// </summary>
  internal static class DefaultSettings
  {
    /// <summary>
    /// Creates the default app settings.
    /// </summary>
    public static AppSettings FillDefaultAppSettings(AppSettings appSettings, IEnvironment environment, IFileSystem fileSystem)
    {
      var settings = new AppSettings(appSettings);
      if(settings.Catalogs == null || settings.Catalogs.Count == 0)
      {
        var defaultCatalogSettings = CreateCatalogSettings("default", environment, fileSystem);
        settings.Catalogs = new Dictionary<string, CatalogSettings>
        {
          { "default", defaultCatalogSettings }
        };
        settings.StartupCatalog = "default";
      }

      if(string.IsNullOrEmpty(settings.StartupCatalog)
        || !settings.Catalogs.ContainsKey(settings.StartupCatalog))
      {
        settings.StartupCatalog = settings.Catalogs.First().Key;
      }

      return settings;
    }

    /// <summary>
    /// Creates the default app engine settings.
    /// </summary>
    public static AppEngineSettings FillDefaultAppEngineSettings(IEnvironment environment, IFileSystem fileSystem)
    {
      var mmDataPath = GetDefaultDataPath(environment, fileSystem);
      var appEngineSettings = new AppEngineSettings(mmDataPath);
      return appEngineSettings;
    }

    /// <summary>
    /// Creates default catalog settings.
    /// </summary>
    public static CatalogSettings CreateCatalogSettings(string catalogName, IEnvironment environment, IFileSystem fileSystem)
    {
      var result = new CatalogSettings
      {
        CatalogName = catalogName,
        ConnectionString = DefaultSettings.CreateDefaultConnectionString(catalogName, environment, fileSystem),
        MediaRoots = new Dictionary<string, string>()
      };

      return result;
    }

    private static string CreateDefaultConnectionString(string catalogName, IEnvironment environment, IFileSystem fileSystem)
    {
      var mmDataPath = GetDefaultDataPath(environment, fileSystem);
      fileSystem.Directory.CreateDirectory(mmDataPath);
      var databaseName = $"{catalogName}.db";
      var defaultDbPath = fileSystem.Path.Combine(mmDataPath, databaseName);

      var connectionString = new SqliteConnectionStringBuilder
      {
        DataSource = defaultDbPath,
      }.ToString();

      return connectionString;
    }

    /// <summary>
    /// Composes the default data path (where sqlite db and lucene indexes located).
    /// </summary>
    private static string GetDefaultDataPath(IEnvironment environment, IFileSystem fileSystem)
    {
      var appDataPath = environment.GetDataPath();
      var mmDataPath = fileSystem.Path.Combine(appDataPath, ".mediafiles");

      return mmDataPath;
    }
  }
}
