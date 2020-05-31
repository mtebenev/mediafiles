using System.Threading.Tasks;
using AppEngine.Video.VideoImprint;

namespace Mt.MediaFiles.AppEngine.Video.VideoImprint
{
  /// <summary>
  /// Common interface for video imprint builders.
  /// </summary>
  public interface IVideoImprintBuilder
  {
    /// <summary>
    /// Creates the video imprint record for given file.
    /// </summary>
    Task<VideoImprintRecord> CreateRecordAsync(int catalogItemId, string fsPath, double videoDuration);
  }
}
