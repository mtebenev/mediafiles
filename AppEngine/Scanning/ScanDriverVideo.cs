using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.CatalogStorage;
using OrchardCore.FileStorage;

namespace Mt.MediaMan.AppEngine.Scanning
{
  /// <summary>
  /// Extracts video information
  /// </summary>
  internal class ScanDriverVideo : IScanDriver
  {
    public Task<bool> IsSupportedAsync(IFileStoreEntry fileStoreEntry)
    {
      throw new System.NotImplementedException();
    }

    public Task ScanAsync(IScanContext scanContext, IFileStoreEntry fileStoreEntry, IItemStorage itemStorage)
    {
      throw new System.NotImplementedException();
    }
  }
}
