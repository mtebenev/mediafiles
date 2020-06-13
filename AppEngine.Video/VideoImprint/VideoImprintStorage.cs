using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using Mt.MediaFiles.AppEngine.Common;

namespace AppEngine.Video.VideoImprint
{
  /// <summary>
  /// The video imprint storage.
  /// Design note: singleton. With current architecture this is really a bottleneck.
  /// Having one storage per service let us get rid of locking when saving records.
  /// Need to seperate the buffer buffer and anctual storage (so we can have buffer per service).
  /// </summary>
  internal class VideoImprintStorage :
    IVideoImprintStorage,
    IBufferedStorage
  {
    private IDbConnection _dbConnection;
    private readonly IDbConnectionProvider _dbConnectionProvider;
    private readonly AsyncSemaphoreLock _semaphore;
    private readonly VideoImprintRecord[] _buffer;
    private int _bufferPosition;

    public VideoImprintStorage(IDbConnectionProvider dbConnectionProvider, int bufferSize)
    {
      this._dbConnectionProvider = dbConnectionProvider;
      this._semaphore = new AsyncSemaphoreLock();
      this._buffer = new VideoImprintRecord[bufferSize];
      this._bufferPosition = 0;
    }

    /// <summary>
    /// IVideoImprintStorage.
    /// </summary>
    public async Task<IList<VideoImprintRecord>> GetAllRecordsAsync()
    {
      this.EnsureDbConnection();
      var query = @"SELECT * FROM VideoImprint";
      var records = await this._dbConnection.QueryAsync<VideoImprintRecord>(query);

      return records.ToList();
    }

    /// <summary>
    /// IVideoImprintStorage.
    /// </summary>
    public async Task SaveRecordAsync(VideoImprintRecord record)
    {
      using(await this._semaphore.Lock())
      {
        if(this._bufferPosition == this._buffer.Length)
        {
          await this.FlushAsync();
        }

        this._buffer[this._bufferPosition] = record;
        this._bufferPosition++;
      }
    }

    /// <summary>
    /// IVideoImprintStorage.
    /// </summary>
    public async Task DeleteRecordsAsync(int catalogItemId)
    {
      this.EnsureDbConnection();
      var query = @"DELETE FROM VideoImprint WHERE CatalogItemId=@CatalogItemId";
      await this._dbConnection.ExecuteAsync(query, new { CatalogItemId = catalogItemId });
    }

    /// <summary>
    /// IVideoImprintStorage.
    /// Note: assumes single-treahded access.
    /// </summary>
    public Task FlushAsync()
    {
      this.EnsureDbConnection();
      if(this._bufferPosition > 0)
      {
        var toSave = this._buffer.Take(this._bufferPosition);
        using(var transaction = this._dbConnection.BeginTransaction())
        {
          foreach(var r in toSave)
          {
            this._dbConnection.Insert(r, transaction);
          }
          transaction.Commit();
        }
        this._bufferPosition = 0;
      }

      return Task.CompletedTask;
    }

    private void EnsureDbConnection()
    {
      if(this._dbConnection == null)
      {
        this._dbConnection = this._dbConnectionProvider.GetConnection();
      }
    }
  }
}
