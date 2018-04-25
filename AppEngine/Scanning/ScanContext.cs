using Mt.MediaMan.AppEngine.CatalogStorage;
using Mt.MediaMan.AppEngine.Search;

namespace Mt.MediaMan.AppEngine.Scanning
{
  internal class ScanContext : IScanContext
  {

    public ScanContext(ScanConfiguration scanConfiguration, IItemStorage itemStorage, LuceneIndexManager indexManager)
    {
      ScanConfiguration = scanConfiguration;
      ItemStorage = itemStorage;
      IndexManager = indexManager;
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
    public LuceneIndexManager IndexManager { get; }
  }
}
