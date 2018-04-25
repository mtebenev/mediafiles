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
      CatalogItemIndexer = new CatalogItemIndexerNull();
    }

    public string Id => "Epub";
    public IScanDriver ScanDriver { get; }
    public ICatalogItemIndexer CatalogItemIndexer { get; }
  }
}
