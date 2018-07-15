using System.Data;
using System.Data.SqlClient;
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
