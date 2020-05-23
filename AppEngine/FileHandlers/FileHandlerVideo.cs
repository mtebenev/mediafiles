using System;
using System.Linq;
using System.Threading.Tasks;
using MediaToolkit.Services;
using Mt.MediaFiles.AppEngine.FileStorage;
using Mt.MediaFiles.AppEngine.Scanning;
using Mt.MediaFiles.AppEngine.Search;

namespace Mt.MediaFiles.AppEngine.FileHandlers
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

    /// <summary>
    /// IFileHandler.
    /// </summary>
    public string Id => HandlerIds.FileHandlerVideo;

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
      var supportedExtensions = new[] { ".mkv", ".mp4", ".avi", ".wmv", ".mpg", ".flv", ".m4v", ".gp3" };
      var result = supportedExtensions.Any(e => fileStoreEntry.Name.EndsWith(e, StringComparison.InvariantCultureIgnoreCase));

      return Task.FromResult(result);
    }
  }
}
