using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Mt.MediaMan.AppEngine.CatalogStorage;
using YesSql.Provider.SqlServer;

namespace Mt.MediaMan.AppEngine
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
      services.AddSingleton<IDbConnection>(x => new SqlConnection(catalogSettings.ConnectionString));
      services.AddSingleton<YesSql.IConfiguration>(x =>
      {
        var storeConfiguration = new YesSql.Configuration();
        storeConfiguration.UseSqlServer(catalogSettings.ConnectionString, IsolationLevel.ReadUncommitted);

        return storeConfiguration;
      });

      services.AddTransient<IStorageManager, StorageManager>();
    }
  }
}
