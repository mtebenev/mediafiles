using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mt.MediaFiles.AppEngine.Video.Thumbnail
{
  /// <summary>
  /// The thumbnail storage interface.
  /// </summary>
  public interface IThumbnailStorage
  {
    /// <summary>
    /// Saves records into the storage.
    /// </summary>
    Task SaveRecordsAsync(IList<ThumbnailRecord> records);

    /// <summary>
    /// Loads records for the catalog item.
    /// </summary>
    Task<IList<ThumbnailRecord>> GetCatalogItemRecordsAsync(int catalogItemId);

    /// <summary>
    /// Retrieves thumbnail IDs for the catalog item.
    /// </summary>
    Task<IList<int>> GetThumbnailIds(int catalogItemId);

    /// <summary>
    /// Retrieves data of the thumbnail by id.
    /// </summary>
    Task<byte[]> GetThumbnailDataAsync(int thumbnailId);
  }
}
