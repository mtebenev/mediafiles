using Microsoft.Extensions.Logging;
using Mt.MediaMan.AppEngine.CatalogStorage;
using Mt.MediaMan.AppEngine.Search;
using Mt.MediaMan.AppEngine.Tasks;

namespace Mt.MediaMan.AppEngine.Scanning
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
