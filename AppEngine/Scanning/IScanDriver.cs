using System.Threading.Tasks;
using Mt.MediaFiles.AppEngine.CatalogStorage;

namespace Mt.MediaFiles.AppEngine.Scanning
{
  /// <summary>
  /// Scan driveres extract file type-specific properties and stores document
  /// </summary>
  internal interface IScanDriver
  {
    /// <summary>
    /// Extracts information and stores it in item storage
    /// </summary>
    Task ScanAsync(int catalogItemId, FileStoreEntryContext fileStoreEntryContext, CatalogItemData catalogItemData);
  }
}
