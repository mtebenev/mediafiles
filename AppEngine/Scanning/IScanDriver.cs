using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.CatalogStorage;

namespace Mt.MediaMan.AppEngine.Scanning
{
  /// <summary>
  /// Scan driveres extract file type-specific properties and stores document
  /// </summary>
  internal interface IScanDriver
  {
    /// <summary>
    /// Extracts information and stores it in item storage
    /// </summary>
    Task ScanAsync(IScanContext scanContext, int catalogItemId, FileStoreEntryContext fileStoreEntryContext, CatalogItemData catalogItemData);
  }
}
