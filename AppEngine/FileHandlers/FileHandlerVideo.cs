using Mt.MediaMan.AppEngine.Scanning;
using Mt.MediaMan.AppEngine.Search;

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
  }
}
