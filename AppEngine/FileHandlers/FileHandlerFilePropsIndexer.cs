using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Scanning;
using Mt.MediaMan.AppEngine.Search;
using OrchardCore.FileStorage;

namespace Mt.MediaMan.AppEngine.FileHandlers
{
  /// <summary>
  /// Indexes file properties
  /// </summary>
  internal class FileHandlerFilePropsIndexer : IFileHandler
  {
    public FileHandlerFilePropsIndexer()
    {
      ScanDriver = new ScanDriverNull();
      CatalogItemIndexer = new CatalogImteIndexerFileProps();
    }

    public string Id => "FilePropsIndexer";
    public IScanDriver ScanDriver { get; }
    public ICatalogItemIndexer CatalogItemIndexer { get; }
    public Task<bool> IsSupportedAsync(IFileStoreEntry fileStoreEntry)
    {
      return Task.FromResult(true); // Should process any entry
    }
  }
}
