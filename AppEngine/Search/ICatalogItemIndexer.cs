using System.Threading.Tasks;

namespace Mt.MediaFiles.AppEngine.Search
{
  /// <summary>
  /// Performs indexing catalog item or info part
  /// </summary>
  internal interface ICatalogItemIndexer
  {
    /// <summary>
    /// Scans a catalog item
    /// </summary>
    Task IndexItemAsync(IIndexingContext indexingContext);
  }
}
