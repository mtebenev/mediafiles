using System.Data;
using System.Threading.Tasks;
using Dapper;
using Mt.MediaMan.AppEngine.CatalogStorage;

namespace AppEngine.Video.VideoImprint
{
  /// <summary>
  /// The DB provider for the videoimprint module.
  /// </summary>
  internal class ModuleDbProvider : IModuleDbProvider
  {
    /// <summary>
    /// IModuleDbProvider
    /// </summary>
    public async Task InitializeDbAsync(IDbConnection dbConnection)
    {
      var isTableExists = await DbUtils.IsTableExistsAsync(dbConnection, "VideoImprint");
      if(!isTableExists)
      {
        // Catalog item table
        var query = @"CREATE TABLE VideoImprint (
    VideoImprintId INTEGER            NOT NULL PRIMARY KEY AUTOINCREMENT,
    CatalogItemId  INTEGER            NOT NULL,
    ImprintData                       BLOB NOT NULL
);";
        await dbConnection.ExecuteAsync(query);
      }
    }
  }
}
