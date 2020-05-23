using System.Data;
using AspNetCoreInjection.TypedFactories;
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
    public static void ConfigureContainer(IServiceCollection services, ICatalogSettings catalogSettings, IDbConnection connection)
    {
      services.AddSingleton<IDbConnection>(connection);
      services.AddSingleton<YesSql.IConfiguration>(x =>
      {
        var storeConfiguration = new YesSql.Configuration();
        storeConfiguration.UseSqLite(catalogSettings.ConnectionString, IsolationLevel.ReadUncommitted);

        return storeConfiguration;
      });

      services.AddTransient<IStorageManager, StorageManager>();
      services.AddTransient<IScanConfigurationBuilder, ScanConfigurationBuilder>();

      // Scan services
      services.AddTransient<IScanService, ScanServiceScanInfo>();

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
  }
}
