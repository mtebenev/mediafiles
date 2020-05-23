using System.Threading.Tasks;

namespace AppEngine.Video.Discovery
{
  /// <summary>
  /// Media object generally represents a media file.
  /// </summary>
  public interface IMediaObject
  {
    Task<string> GetFsPathAsync();
  }
}
