using System;
using System.Data;
using AspNetCoreInjection.TypedFactories;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using Mt.MediaFiles.AppEngine.FileHandlers;
using Mt.MediaFiles.AppEngine.Scanning;
using Mt.MediaFiles.AppEngine.Tasks;
using YesSql.Provider.Sqlite;

namespace Mt.MediaFiles.AppEngine
{
  /// <summary>
  /// The AppEngine mdoule.
  /// </summary>
  public static class AppEngineModule
  {
    /// <summary>
    /// Call to configure the container.
    /// </summary>
    public static void ConfigureContainer(IServiceCollection services, ICatalogSettings catalogSettings)
    {
      services.AddSingleton<YesSql.IConfiguration>(x =>
      {
        var storeConfiguration = new YesSql.Configuration();
        storeConfiguration.UseSqLite(catalogSettings.ConnectionString, IsolationLevel.ReadUncommitted);

        return storeConfiguration;
      });
      services.AddTransient<IDbConnection>(c => OpenDbConnection(catalogSettings));

      services.AddTransient<IStorageManager, StorageManager>();
      services.AddTransient<IScanConfigurationBuilder, ScanConfigurationBuilder>();

      // Scan services
      services.AddTransient<IScanServiceFactory, ScanServiceFactoryScanInfo>();

      // File handlers
      services.AddTransient<IFileHandler, FileHandlerVideo>();

      // Typed factories
      services
        .RegisterTypedFactory<ICatalogTaskCheckStatusFactory>().ForConcreteType<CatalogTaskCheckStatus>();
      services
        .RegisterTypedFactory<ICatalogTaskScanFactory>().ForConcreteType<CatalogTaskScan>();
      services
        .RegisterTypedFactory<IItemScannerFactory>().ForConcreteType<ItemScanner>();
    }

    /// <summary>
    /// Initializes connection.
    /// </summary>
    private static IDbConnection OpenDbConnection(ICatalogSettings catalogSettings)
    {
      IDbConnection result = null;
      try
      {
        result = new SqliteConnection(catalogSettings.ConnectionString);
        result.Open();
      }
      catch(Exception e)
      {
        throw new InvalidOperationException(
          $"Could not open the sqlite database \"{catalogSettings.ConnectionString}\". Please make sure that the specified directory exists",
          e
        );
      }

      return result;
    }
  }
}
