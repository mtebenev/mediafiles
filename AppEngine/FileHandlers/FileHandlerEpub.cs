using System;
using System.Linq;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Scanning;
using Mt.MediaMan.AppEngine.Search;
using OrchardCore.FileStorage;

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
      CatalogItemIndexer = new CatalogItemIndexerNull();
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
