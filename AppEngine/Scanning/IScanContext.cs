using Microsoft.Extensions.Logging;
using Mt.MediaMan.AppEngine.CatalogStorage;
using Mt.MediaMan.AppEngine.Search;

namespace Mt.MediaMan.AppEngine.Scanning
{
  /// <summary>
  /// Contextual objects for scan process
  /// </summary>
  internal interface IScanContext
  {
    LuceneIndexManager IndexManager { get; }
    ScanConfiguration ScanConfiguration { get; }
    IItemStorage ItemStorage { get; }
    ILogger Logger { get; }
  }
}
