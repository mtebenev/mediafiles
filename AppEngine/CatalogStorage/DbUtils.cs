using Dapper;
using System.Data;
using System.Threading.Tasks;
using System.Linq;

namespace Mt.MediaFiles.AppEngine.CatalogStorage
{
  /// <summary>
  /// Helpers for working with the database.
  /// </summary>
  public static class DbUtils
  {
    public static async Task<bool> IsTableExistsAsync(IDbConnection dbConnection, string tableName)
    {
      var checkTableQuery = $@"SELECT name FROM sqlite_master WHERE type='table' AND name='{tableName}'";
      var tables = await dbConnection.QueryAsync(checkTableQuery);
      var result = tables.Any();

      return result;
    }
  }
}
