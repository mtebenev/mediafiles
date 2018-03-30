using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.CatalogStorage;
using OrchardCore.FileStorage;

namespace Mt.MediaMan.AppEngine.Scanning
{
  /// <summary>
  /// Scan driveres extract file type-specific properties and stores document
  /// </summary>
  internal interface IScanDriver
  {
    /// <summary>
    /// Call to check if the file is supported by the driver
    /// </summary>
    Task<bool> IsSupportedAsync(IFileStoreEntry fileStoreEntry);

    /// <summary>
    /// Extracts information and stores it in item storage
    /// </summary>
    Task ScanAsync(IScanContext scanContext, int catalogItemId, IFileStoreEntry fileStoreEntry, IItemStorage itemStorage);
  }
}
