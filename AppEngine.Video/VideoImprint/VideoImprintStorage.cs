using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Mt.MediaFiles.AppEngine.Common;

namespace AppEngine.Video.VideoImprint
{
  /// <summary>
  /// The video imprint storage.
  /// Design note: singleton.
  /// </summary>
  internal class VideoImprintStorage : IVideoImprintStorage
  {
    private readonly IDbConnection _dbConnection;
    private readonly AsyncSemaphoreLock _semaphore;

    public VideoImprintStorage(IDbConnection dbConnection)
    {
      this._dbConnection = dbConnection;
      this._semaphore = new AsyncSemaphoreLock();
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
    public async Task SaveRecordsAsync(IEnumerable<VideoImprintRecord> records)
    {
      using(await this._semaphore.Lock())
      {
        using(var transaction = this._dbConnection.BeginTransaction())
        {
          foreach(var r in records)
          {
            this._dbConnection.Insert(r, transaction);
          }
          transaction.Commit();
        }
      }
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
