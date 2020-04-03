using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.Sqlite;
using YesSql;

namespace Mt.MediaMan.AppEngine.CatalogStorage
{
  internal class StorageManager : IStorageManager
  {
    private readonly IDbConnection _dbConnection;
    private readonly Store _store;

    /// <summary>
    /// Create with connection string
    /// </summary>
    public StorageManager(IDbConnection dbConnection, IConfiguration storeConfiguration)
    {
      this._dbConnection = dbConnection;
      this._store = new Store(storeConfiguration);
      this._store.RegisterIndexes<CatalogItemIndexProvider>();
    }

    /// <summary>
    /// Use to reset all tables in the SQL storage
    /// TODO: Modules should delete their own data.
    /// </summary>
    public static async Task ResetStorage(string connectionString)
    {
      var dbConnection = new SqliteConnection(connectionString);
      var tables = new[]
      {
        "CatalogItem",
        "MapIndexCatalogItem",
        "MapIndexEbook",
        "Document",
        "Identifiers",
        "VideoImprint"
      };
      foreach (var tableName in tables)
      {
        await dbConnection.ExecuteAsync($"DROP TABLE {tableName}");
      }
    }

    public IDbConnection DbConnection => _dbConnection;
    public IStore Store => _store;

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
