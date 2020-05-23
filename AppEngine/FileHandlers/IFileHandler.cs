using System.Threading.Tasks;
using Mt.MediaFiles.AppEngine.FileStorage;
using Mt.MediaFiles.AppEngine.Scanning;
using Mt.MediaFiles.AppEngine.Search;

namespace Mt.MediaFiles.AppEngine.FileHandlers
{
  /// <summary>
  /// Abstract factory for file handlers.
  /// </summary>
  internal interface IFileHandler
  {
    /// <summary>
    /// Unique id of the file handler.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// The scan driver extracts file properties.
    /// </summary>
    IScanDriver ScanDriver { get; }

    /// <summary>
    /// The item indexer store searchable properties in the index.
    /// </summary>
    ICatalogItemIndexer CatalogItemIndexer { get; }

    /// <summary>
    /// Will return true if the scan entry should be processed by the file handler
    /// </summary>
    Task<bool> IsSupportedAsync(IFileStoreEntry fileStoreEntry);
  }
}
