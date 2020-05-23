using Microsoft.Extensions.Logging;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using Mt.MediaFiles.AppEngine.Search;
using Mt.MediaFiles.AppEngine.Tasks;

namespace Mt.MediaFiles.AppEngine.Scanning
{
  /// <summary>
  /// Contextual objects for scan process.
  /// Design note. This is an internal interface for using inside only in this core assembly.
  /// It provides access to the very intimate core stuff.
  /// Extensible classes should obtain narrow contexts.
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
