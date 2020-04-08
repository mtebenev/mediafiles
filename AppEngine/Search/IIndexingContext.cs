using Mt.MediaFiles.AppEngine.CatalogStorage;

namespace Mt.MediaFiles.AppEngine.Search
{
  /// <summary>
  /// Provides contextual information for indexing
  /// </summary>
  internal interface IIndexingContext
  {
    /// <summary>
    /// Index for the document
    /// </summary>
    DocumentIndex DocumentIndex { get; }

    /// <summary>
    /// Base item properties
    /// </summary>
    CatalogItemRecord ItemRecord { get; }

    /// <summary>
    /// Scanned data (info parts)
    /// </summary>
    CatalogItemData CatalogItemData { get; }
  }
}
