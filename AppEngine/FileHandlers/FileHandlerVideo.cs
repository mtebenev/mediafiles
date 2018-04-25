using System;
using System.Linq;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Scanning;
using Mt.MediaMan.AppEngine.Search;
using OrchardCore.FileStorage;

namespace Mt.MediaMan.AppEngine.FileHandlers
{
  /// <summary>
  /// Video-related file handler
  /// </summary>
  internal class FileHandlerVideo : IFileHandler
  {
    public FileHandlerVideo()
    {
      ScanDriver = new ScanDriverVideo();
      CatalogItemIndexer = new CatalogItemIndexerNull();
    }

    public string Id => "Video";
    public IScanDriver ScanDriver { get; }
    public ICatalogItemIndexer CatalogItemIndexer { get; }

    public Task<bool> IsSupportedAsync(IFileStoreEntry fileStoreEntry)
    {
      var supportedExtensions = new[] {".mkv", ".mp4", ".avi"};
      var result = supportedExtensions.Any(e => fileStoreEntry.Name.EndsWith(e, StringComparison.InvariantCultureIgnoreCase));

      return Task.FromResult(result);
    }
  }
}
