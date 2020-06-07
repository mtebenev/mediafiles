using Dapper;
using Dapper.Contrib.Extensions;
using Mt.MediaFiles.AppEngine.Common;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mt.MediaFiles.AppEngine.Video.Thumbnail
{
  /// <summary>
  /// The thumbnail storage.
  /// </summary>
  internal class ThumbnailStorage : IThumbnailStorage
  {
    private readonly IDbConnection _dbConnection;
    private readonly AsyncSemaphoreLock _semaphore;

    /// <summary>
    /// Ctor.
    /// </summary>
    public ThumbnailStorage(IDbConnection dbConnection)
    {
      this._dbConnection = dbConnection;
      this._semaphore = new AsyncSemaphoreLock();
    }

    /// <summary>
    /// IThumbnailStorage.
    /// </summary>
    public async Task SaveRecordsAsync(IList<ThumbnailRecord> records)
    {
      using(await this._semaphore.Lock())
      using(var transaction = this._dbConnection.BeginTransaction())
      {
        foreach (var r in records)
        {
          this._dbConnection.Insert(r, transaction);
        }
        transaction.Commit();
      }
    }

    /// <summary>
    /// IThumbnailStorage.
    /// </summary>
    public async Task<IList<ThumbnailRecord>> GetCatalogItemRecordsAsync(int catalogItemId)
    {
      var query = @"SELECT * FROM Thumbnail WHERE CatalogItemId=@CatalogItemId";
      var records = await this._dbConnection.QueryAsync<ThumbnailRecord>(query, new { CatalogItemId = catalogItemId });

      return records.ToList();
    }
  }
}
