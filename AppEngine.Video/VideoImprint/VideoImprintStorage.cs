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
    public async Task<IList<VideoImprintRecord>> GetAllRecordsAsync()
    {
      var query = @"SELECT * FROM VideoImprint";
      var records = await this._dbConnection.QueryAsync<VideoImprintRecord>(query);

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
