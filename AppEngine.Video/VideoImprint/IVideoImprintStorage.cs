using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppEngine.Video.VideoImprint
{
  /// <summary>
  /// The client interface for the video imprint storage.
  /// </summary>
  public interface IVideoImprintStorage
  {
    /// <summary>
    /// Retrieves all stored video imprints.
    /// </summary>
    Task<IList<VideoImprintRecord>> GetAllRecordsAsync();


    /// <summary>
    /// Saves the record in the storage.
    /// </summary>
    Task SaveRecordAsync(VideoImprintRecord record);

    /// <summary>
    /// Deletes all records for a catalog item.
    /// </summary>
    Task DeleteRecordsAsync(int catalogItemId);
  }
}
