using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using Microsoft.Data.Sqlite;
using Mt.MediaMan.ClientApp.Cli.Configuration;

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
    public static AppSettings FillDefaultSettings(AppSettings appSettings, IEnvironment environment, IFileSystem fileSystem)
    {
      var settings = new AppSettings(appSettings);
      if(settings.Catalogs == null || settings.Catalogs.Count == 0)
      {
        var defaultCatalogSettings = new CatalogSettings
        {
          CatalogName = "default",
          ConnectionString = DefaultSettings.CreateDefaultConnectionString("default", environment, fileSystem),
          MediaRoots = new Dictionary<string, string>()
        };

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

    private static string CreateDefaultConnectionString(string catalogName, IEnvironment environment, IFileSystem fileSystem)
    {
      var appDataPath = environment.GetDataPath();
      var mmDataPath = fileSystem.Path.Combine(appDataPath, ".mediaman");
      fileSystem.Directory.CreateDirectory(mmDataPath);
      var databaseName = $"{catalogName}.db";
      var defaultDbPath = fileSystem.Path.Combine(mmDataPath, databaseName);

      var connectionString = new SqliteConnectionStringBuilder
      {
        DataSource = defaultDbPath,
      }.ToString();

      return connectionString;
    }

  }
}
