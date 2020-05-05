using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;

namespace AppEngine.Video.VideoImprint
{
  /// <summary>
  /// The video imprint storage.
  /// </summary>
  internal class VideoImprintStorage : IVideoImprintStorage
  {
    private readonly IDbConnection _dbConnection;

    public VideoImprintStorage(IDbConnection dbConnection)
    {
      this._dbConnection = dbConnection;
    }

    /// <summary>
    /// IVideoImprintStorage.
    /// </summary>
    public async Task<IReadOnlyList<int>> GetCatalogItemIdsAsync()
    {
      var query = @"SELECT DISTINCT CatalogItemId FROM VideoImprint";
      var catalogItemIds = await this._dbConnection.QueryAsync<int>(query);

      return catalogItemIds.ToList();
    }

    /// <summary>
    /// IVideoImprintStorage.
    /// </summary>
    public async Task<IList<VideoImprintRecord>> GetRecordsAsync(int catalogItemId)
    {
      var query = @"SELECT * from VideoImprint WHERE CatalogItemId=@CatalogItemId";
      var records = await this._dbConnection.QueryAsync<VideoImprintRecord>(query, new { CatalogItemId = catalogItemId });

      return records.ToList();
    }

    /// <summary>
    /// IVideoImprintStorage.
    /// </summary>
    public Task SaveRecordsAsync(IEnumerable<VideoImprintRecord> records)
    {
      using(var transaction = this._dbConnection.BeginTransaction())
      {
        foreach(var r in records)
        {
          this._dbConnection.Insert(r, transaction);
        }
        transaction.Commit();
      }

      return Task.CompletedTask;
    }

    /// <summary>
    /// IVideoImprintStorage.
    /// </summary>
    public async Task DeleteRecordsAsync(int catalogItemId)
    {
      var query = @"DELETE FROM VideoImprint WHERE CatalogItemId=@CatalogItemId";
      await this._dbConnection.ExecuteAsync(query, new { CatalogItemId = catalogItemId });
    }

    /// <summary>
    /// IVideoImprintStorage.
    /// </summary>
    public Task FlushAsync()
    {
      return Task.CompletedTask;
    }
  }
}
