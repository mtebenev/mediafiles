using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mt.MediaFiles.AppEngine.Video.Thumbnail
{
  /// <summary>
  /// The thumbnail storage interface.
  /// </summary>
  internal interface IThumbnailStorage
  {
    /// <summary>
    /// Saves records into the storage.
    /// </summary>
    Task SaveRecordsAsync(IList<ThumbnailRecord> records);
  }
}
