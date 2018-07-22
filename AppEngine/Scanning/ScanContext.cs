using Microsoft.Extensions.Logging;
using Mt.MediaMan.AppEngine.CatalogStorage;
using Mt.MediaMan.AppEngine.Search;

namespace Mt.MediaMan.AppEngine.Scanning
{
  internal class ScanContext : IScanContext
  {

    public ScanContext(ScanConfiguration scanConfiguration, IItemStorage itemStorage, LuceneIndexManager indexManager, ILoggerFactory loggerFactory)
    {
      ScanConfiguration = scanConfiguration;
      ItemStorage = itemStorage;
      IndexManager = indexManager;
      Logger = loggerFactory.CreateLogger("Scanning");
    }

    /// <summary>
    /// IScanContext
    /// </summary>
    public ScanConfiguration ScanConfiguration { get; }

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
  }
}
