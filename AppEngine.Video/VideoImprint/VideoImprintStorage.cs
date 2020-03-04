using System.Data;
using System.Threading.Tasks;
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
    /// IVideoImprintStorage
    /// </summary>
    public Task SaveRecordAsync(VideoImprintRecord imprintRecord)
    {
      var result = this._dbConnection.InsertAsync(imprintRecord);
      return result;
    }
  }
}
