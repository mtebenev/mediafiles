using System;
using System.Linq;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.FileStorage;
using Mt.MediaMan.AppEngine.Scanning;
using Mt.MediaMan.AppEngine.Search;

namespace Mt.MediaMan.AppEngine.FileHandlers
{
  /// <summary>
  /// Epub-related file handler
  /// </summary>
  internal class FileHandlerEpub : IFileHandler
  {
    public FileHandlerEpub()
    {
      ScanDriver = new ScanDriverEpub();
      CatalogItemIndexer = new InfoPartIndexerBook();
    }

    public string Id => "Epub";
    public IScanDriver ScanDriver { get; }
    public ICatalogItemIndexer CatalogItemIndexer { get; }

    public Task<bool> IsSupportedAsync(IFileStoreEntry fileStoreEntry)
    {
      var supportedExtensions = new[] {".epub"};
      var result = supportedExtensions.Any(e => fileStoreEntry.Name.EndsWith(e, StringComparison.InvariantCultureIgnoreCase));

      return Task.FromResult(result);
    }
  }
}
