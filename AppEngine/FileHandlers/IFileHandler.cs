using Mt.MediaMan.AppEngine.Scanning;
using Mt.MediaMan.AppEngine.Search;

namespace Mt.MediaMan.AppEngine.FileHandlers
{
  /// <summary>
  /// Abstract factory for file handlers
  /// </summary>
  internal interface IFileHandler
  {
    /// <summary>
    /// Unique ID of the handler
    /// </summary>
    string Id { get; }

    IScanDriver ScanDriver { get; }
    ICatalogItemIndexer CatalogItemIndexer { get; }
  }
}
