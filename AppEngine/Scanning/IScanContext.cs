using Microsoft.Extensions.Logging;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using Mt.MediaFiles.AppEngine.Search;
using Mt.MediaFiles.AppEngine.Tasks;

namespace Mt.MediaFiles.AppEngine.Scanning
{
  /// <summary>
  /// Contextual objects for scan process
  /// </summary>
  internal interface IScanContext
  {
    LuceneIndexManager IndexManager { get; }
    IScanConfiguration ScanConfiguration { get; }
    IItemStorage ItemStorage { get; }
    ILogger Logger { get; }
    IProgressOperation ProgressOperation { get; }
  }
}
