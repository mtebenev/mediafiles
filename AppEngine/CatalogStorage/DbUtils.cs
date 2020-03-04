using Dapper;
using System.Data;
using System.Threading.Tasks;
using System.Linq;

namespace Mt.MediaMan.AppEngine.CatalogStorage
{
  /// <summary>
  /// Helpers for working with the database.
  /// </summary>
  public static class DbUtils
  {
    public static async Task<bool> IsTableExistsAsync(IDbConnection dbConnection, string tableName)
    {
      var checkTableQuery = $@"SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = '{tableName}'";
      var tables = await dbConnection.QueryAsync(checkTableQuery);
      var result = tables.Any();

      return result;
    }
  }
}
