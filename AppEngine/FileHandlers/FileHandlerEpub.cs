using System;
using System.Linq;
using System.Threading.Tasks;
using Mt.MediaFiles.AppEngine.FileStorage;
using Mt.MediaFiles.AppEngine.Scanning;
using Mt.MediaFiles.AppEngine.Search;

namespace Mt.MediaFiles.AppEngine.FileHandlers
{
  /// <summary>
  /// Epub-related file handler
  /// </summary>
  internal class FileHandlerEpub : IFileHandler
  {
    /// <summary>
    /// Ctor.
    /// </summary>
    public FileHandlerEpub()
    {
      ScanDriver = new ScanDriverEpub();
      CatalogItemIndexer = new InfoPartIndexerBook();
    }

    /// <summary>
    /// IFileHandler.
    /// </summary>
    public string Id => "Epub";

    /// <summary>
    /// IFileHandler.
    /// </summary>
    public IScanDriver ScanDriver { get; }

    /// <summary>
    /// IFileHandler.
    /// </summary>
    public ICatalogItemIndexer CatalogItemIndexer { get; }

    /// <summary>
    /// IFileHandler.
    /// </summary>
    public Task<bool> IsSupportedAsync(IFileStoreEntry fileStoreEntry)
    {
      var supportedExtensions = new[] { ".epub" };
      var result = supportedExtensions.Any(e => fileStoreEntry.Name.EndsWith(e, StringComparison.InvariantCultureIgnoreCase));

      return Task.FromResult(result);
    }
  }
}
