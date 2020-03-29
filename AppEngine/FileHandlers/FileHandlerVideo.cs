using System;
using System.Linq;
using System.Threading.Tasks;
using MediaToolkit.Services;
using Mt.MediaMan.AppEngine.FileStorage;
using Mt.MediaMan.AppEngine.Scanning;
using Mt.MediaMan.AppEngine.Search;

namespace Mt.MediaMan.AppEngine.FileHandlers
{
  /// <summary>
  /// Video-related file handler
  /// </summary>
  internal class FileHandlerVideo : IFileHandler
  {
    public FileHandlerVideo(IMediaToolkitService mediaToolkitService)
    {
      ScanDriver = new ScanDriverVideo(mediaToolkitService);
      CatalogItemIndexer = new CatalogItemIndexerNull();
    }

    public string Id => "Video";
    public IScanDriver ScanDriver { get; }
    public ICatalogItemIndexer CatalogItemIndexer { get; }

    public Task<bool> IsSupportedAsync(IFileStoreEntry fileStoreEntry)
    {
      var supportedExtensions = new[] {".mkv", ".mp4", ".avi", ".wmv", ".mpg", ".flv", ".m4v", ".gp3"};
      var result = supportedExtensions.Any(e => fileStoreEntry.Name.EndsWith(e, StringComparison.InvariantCultureIgnoreCase));

      return Task.FromResult(result);
    }
  }
}
