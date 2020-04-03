using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.FileStorage;
using Mt.MediaMan.AppEngine.Scanning;
using Mt.MediaMan.AppEngine.Search;

namespace Mt.MediaMan.AppEngine.FileHandlers
{
  /// <summary>
  /// Abstract factory for file handlers.
  /// </summary>
  internal interface IFileHandler
  {
    /// <summary>
    /// Unique ID of the handler
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
