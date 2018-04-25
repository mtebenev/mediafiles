using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Scanning;
using Mt.MediaMan.AppEngine.Search;
using OrchardCore.FileStorage;

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

    /// <summary>
    /// Will return true if the scan entry should be processed by the file handler
    /// </summary>
    Task<bool> IsSupportedAsync(IFileStoreEntry fileStoreEntry);
  }
}
