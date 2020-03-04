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
        var query = @"CREATE TABLE [dbo].[VideoImprint] (
    [VideoImprintId] INT            IDENTITY (1, 1) NOT NULL,
    [CatalogItemId]  INT            NOT NULL,
    [Data]           VARBINARY(8000) NOT NULL
);";
        await dbConnection.ExecuteAsync(query);
      }
    }
  }
}
