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
    /// Retrieves IDs of all items in the catalog having stored video imprints.
    /// </summary>
    /// <returns></returns>
    Task<IReadOnlyList<int>> GetCatalogItemIdsAsync();

    /// <summary>
    /// Loads all records for the given catalog item from the storage.
    /// </summary>
    Task<IList<VideoImprintRecord>> GetRecordsAsync(int catalogItemId);

    /// <summary>
    /// Saves the record in the storage.
    /// </summary>
    Task SaveRecordsAsync(IEnumerable<VideoImprintRecord> records);

    /// <summary>
    /// Deletes all records for a catalog item.
    /// </summary>
    Task DeleteRecordsAsync(int catalogItemId);
  }
}
