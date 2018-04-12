namespace Mt.MediaMan.AppEngine.Search
{
  /// <summary>
  /// Provides contextual information for indexing
  /// </summary>
  internal interface IIndexingContext
  {
    /// <summary>
    /// Indexed document ID (catalog item ID)
    /// </summary>
    string Key { get; }

    /// <summary>
    /// Index for the document
    /// </summary>
    DocumentIndex DocumentIndex { get; }
  }
}
