using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using Mt.MediaFiles.AppEngine.Tasks;

namespace Mt.MediaFiles.AppEngine.Scanning
{
  /// <summary>
  /// The context for scan services execution.
  /// </summary>
  public interface IScanServiceContext
  {
    ILogger Logger { get; }
    IProgressOperation ProgressOperation { get; }

    /// <summary>
    /// Returns the item data.
    /// </summary>
    CatalogItemData GetItemData();

    /// <summary>
    /// Use to obtain the file store context.
    /// </summary>
    Task<FileStoreEntryContext> GetFileStoreEntryContextAsync();
  }
}
