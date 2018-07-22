using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using YesSql;
using YesSql.Provider.SqlServer;

namespace Mt.MediaMan.AppEngine.CatalogStorage
{
  internal class StorageManager : IStorageManager
  {
    private readonly IDbConnection _dbConnection;
    private readonly Store _store;

    /// <summary>
    /// Create with connection string
    /// </summary>
    public StorageManager(string connectionString)
    {
      _dbConnection = new SqlConnection(connectionString);

      // Document store
      var storeConfiguration = new Configuration();
      storeConfiguration.UseSqlServer(connectionString, IsolationLevel.ReadUncommitted);

      _store = new Store(storeConfiguration);
      _store.RegisterIndexes<CatalogItemIndexProvider>();
    }

    /// <summary>
    /// Use to reset all tables in the SQL storage
    /// </summary>
    public static async Task ResetStorage(string connectionString)
    {
      var dbConnection = new SqlConnection(connectionString);
      var tables = new[]
      {
        "[dbo].[CatalogItem]",
        "[dbo].[MapIndexCatalogItem]",
        "[dbo].[MapIndexEbook]",
        "[dbo].[Document]",
        "[dbo].[Identifiers]"
      };
      foreach (var tableName in tables)
      {
        await dbConnection.ExecuteAsync($"DROP TABLE {tableName}");
      }
    }

    public IDbConnection DbConnection => _dbConnection;
    public Store Store => _store;

    /// <summary>
    /// IDisposable
    /// </summary>
    public void Dispose()
    {
      _dbConnection?.Dispose();
      _store?.Dispose();
    }

  }
}
