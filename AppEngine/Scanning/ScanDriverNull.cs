using System.Threading.Tasks;
using Mt.MediaFiles.AppEngine.CatalogStorage;

namespace Mt.MediaFiles.AppEngine.Scanning
{
  /// <summary>
  /// Scans nothing.
  /// </summary>
  internal class ScanDriverNull : IScanDriver
  {
    public Task ScanAsync(int catalogItemId, FileStoreEntryContext fileStoreEntryContext, CatalogItemData catalogItemData)
    {
      return Task.CompletedTask;
    }
  }
}
