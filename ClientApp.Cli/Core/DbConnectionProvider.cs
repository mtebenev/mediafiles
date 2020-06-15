using Microsoft.Data.Sqlite;
using Mt.MediaFiles.AppEngine.Common;
using System;
using System.Data;
using YesSql;
using YesSql.Provider.Sqlite;

namespace Mt.MediaFiles.ClientApp.Cli.Core
{
  /// <summary>
  /// The client app db provider implementaiton.
  /// </summary>
  internal class DbConnectionProvider :
    IDbConnectionProvider,
    IDbConnectionSource
  {
    private string _connectionString;

    /// <summary>
    /// IDbConnectionProvider.
    /// </summary>
    public IDbConnection GetConnection()
    {
      IDbConnection result = null;
      try
      {
        result = new SqliteConnection(this._connectionString);
        result.Open();
      }
      catch(Exception e)
      {
        throw new InvalidOperationException(
          $"Could not open the sqlite database \"{this._connectionString}\". Please make sure that the specified directory exists",
          e
        );
      }

      return result;
    }

    /// <summary>
    /// IDbConnectionProvider.
    /// </summary>
    public YesSql.IConfiguration GetYesSqlConfiguration()
    {
      var storeConfiguration = new YesSql.Configuration();
      storeConfiguration.UseSqLite(this._connectionString, IsolationLevel.ReadUncommitted);

      return storeConfiguration;
    }

    /// <summary>
    /// IDbConnectionSource.
    /// </summary>
    public void SetConnectionString(string connectionString)
    {
      this._connectionString = connectionString;
    }
  }
}
