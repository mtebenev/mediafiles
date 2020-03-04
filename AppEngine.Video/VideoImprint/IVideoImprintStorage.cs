using System.Threading.Tasks;

namespace AppEngine.Video.VideoImprint
{
  /// <summary>
  /// The client interface for the video imprint storage.
  /// </summary>
  public interface IVideoImprintStorage
  {
    /// <summary>
    /// Saves the record in the storage.
    /// </summary>
    Task SaveRecordAsync(VideoImprintRecord imprintRecord);
  }
}
