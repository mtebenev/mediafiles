using Microsoft.Extensions.Logging;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using Mt.MediaFiles.AppEngine.Search;
using Mt.MediaFiles.AppEngine.Tasks;

namespace Mt.MediaFiles.AppEngine.Scanning
{
  internal class ScanContext : IScanContext
  {

    public ScanContext(IScanConfiguration scanConfiguration, IItemStorage itemStorage, LuceneIndexManager indexManager, ILoggerFactory loggerFactory, IProgressOperation progressOperation)
    {
      ScanConfiguration = scanConfiguration;
      ItemStorage = itemStorage;
      IndexManager = indexManager;
      ProgressOperation = progressOperation;
      Logger = loggerFactory.CreateLogger("Scanning");
    }

    /// <summary>
    /// IScanContext
    /// </summary>
    public IScanConfiguration ScanConfiguration { get; }

    /// <summary>
    /// IScanContext
    /// </summary>
    public IItemStorage ItemStorage { get; }

    /// <summary>
    /// IScanContext
    /// </summary>
    public ILogger Logger { get; }

    /// <summary>
    /// IScanContext
    /// </summary>
    public LuceneIndexManager IndexManager { get; }

    /// <summary>
    /// IScanContext
    /// </summary>
    public IProgressOperation ProgressOperation { get; }
  }
}
