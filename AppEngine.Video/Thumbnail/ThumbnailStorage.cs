using Dapper.Contrib.Extensions;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Mt.MediaFiles.AppEngine.Video.Thumbnail
{
  /// <summary>
  /// The thumbnail storage.
  /// </summary>
  internal class ThumbnailStorage : IThumbnailStorage
  {
    private readonly IDbConnection _dbConnection;

    /// <summary>
    /// Ctor.
    /// </summary>
    public ThumbnailStorage(IDbConnection dbConnection)
    {
      this._dbConnection = dbConnection;
    }

    /// <summary>
    /// IThumbnailStorage.
    /// </summary>
    public Task SaveRecordsAsync(IList<ThumbnailRecord> records)
    {
      using (var transaction = this._dbConnection.BeginTransaction())
      {
        foreach (var r in records)
        {
          this._dbConnection.Insert(r, transaction);
        }
        transaction.Commit();
      }

      return Task.CompletedTask;
    }
  }
}
