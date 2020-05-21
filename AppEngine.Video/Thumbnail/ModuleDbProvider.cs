using Dapper;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using System.Data;
using System.Threading.Tasks;

namespace Mt.MediaFiles.AppEngine.Video.Thumbnail
{
  /// <summary>
  /// The DB provider for the thumbnail module.
  /// </summary>
  internal class ModuleDbProvider : IModuleDbProvider
  {
    /// <summary>
    /// IModuleDbProvider.
    /// </summary>
    public async Task InitializeDbAsync(IDbConnection dbConnection)
    {
      var isTableExists = await DbUtils.IsTableExistsAsync(dbConnection, "Thumbnail");
      if (!isTableExists)
      {
        // Catalog item table
        var query = @"CREATE TABLE Thumbnail (
    ThumbnailId    INTEGER            NOT NULL PRIMARY KEY AUTOINCREMENT,
    CatalogItemId  INTEGER            NOT NULL,
    Position       INTEGER            NOT NULL,
    ThumbnailData  BLOB NOT NULL
);";
        await dbConnection.ExecuteAsync(query);
      }
    }
  }
}
