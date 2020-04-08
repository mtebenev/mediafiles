using System.Data;
using AspNetCoreInjection.TypedFactories;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using Mt.MediaFiles.AppEngine.Scanning;
using Mt.MediaFiles.AppEngine.Tasks;
using Mt.MediaMan.AppEngine.Scanning;
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
      services.AddSingleton<IDbConnection>(x => new SqliteConnection(catalogSettings.ConnectionString));
      services.AddSingleton<YesSql.IConfiguration>(x =>
      {
        var storeConfiguration = new YesSql.Configuration();
        storeConfiguration.UseSqLite(catalogSettings.ConnectionString, IsolationLevel.ReadUncommitted);

        return storeConfiguration;
      });

      services.AddTransient<IStorageManager, StorageManager>();
      services.AddTransient<IScanConfigurationBuilder, ScanConfigurationBuilder>();

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
