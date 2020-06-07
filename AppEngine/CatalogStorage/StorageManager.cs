using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.Sqlite;
using YesSql;

namespace Mt.MediaFiles.AppEngine.CatalogStorage
{
  /// <summary>
  /// Storage manager implementation.
  /// </summary>
  internal class StorageManager : IStorageManager, IDisposable
  {
    private readonly IDbConnection _dbConnection;
    private readonly Store _store;

    /// <summary>
    /// Ctor.
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
        "VideoImprint",
        "Thumbnail"
      };
      foreach(var tableName in tables)
      {
        await dbConnection.ExecuteAsync($"DROP TABLE {tableName}");
      }
    }

    /// <summary>
    /// IStorageManager.
    /// </summary>
    public IDbConnection DbConnection => _dbConnection;

    /// <summary>
    /// IStorageManager.
    /// </summary>
    public IStore Store => _store;

    /// <summary>
    /// IDisposable
    /// </summary>
    public void Dispose()
    {
      _store.Dispose();
    }
  }
}
