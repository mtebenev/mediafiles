using AspNetCoreInjection.TypedFactories;
using Microsoft.Extensions.DependencyInjection;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using Mt.MediaFiles.AppEngine.Common;
using Mt.MediaFiles.AppEngine.FileHandlers;
using Mt.MediaFiles.AppEngine.Scanning;
using Mt.MediaFiles.AppEngine.Tasks;

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
    public static void ConfigureContainer(IServiceCollection services)
    {
      services.AddSingleton<YesSql.IConfiguration>(c => c.GetRequiredService<IDbConnectionProvider>().GetYesSqlConfiguration());

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
  }
}
