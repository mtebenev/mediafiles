using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.CatalogStorage;

namespace Mt.MediaMan.AppEngine.Scanning
{
  /// <summary>
  /// Scans nothing
  /// </summary>
  internal class ScanDriverNull : IScanDriver
  {
    public Task ScanAsync(IScanContext scanContext, int catalogItemId, FileStoreEntryContext fileStoreEntryContext, CatalogItemData catalogItemData)
    {
      return Task.CompletedTask;
    }
  }
}
