using Microsoft.Extensions.Logging;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using Mt.MediaFiles.AppEngine.Search;
using Mt.MediaFiles.AppEngine.Tasks;

namespace Mt.MediaFiles.AppEngine.Scanning
{
  /// <summary>
  /// The scan context implementation.
  /// </summary>
  internal class ScanContext : IScanContext
  {
    private readonly ITaskExecutionContext _taskExecutionContext;

    /// <summary>
    /// Ctor.
    /// </summary>
    public ScanContext(
      ITaskExecutionContext taskExecutionContext,
      IScanConfiguration scanConfiguration,
      IItemStorage itemStorage,
      LuceneIndexManager indexManager)
    {
      this._taskExecutionContext = taskExecutionContext;
      this.ScanConfiguration = scanConfiguration;
      this.ItemStorage = itemStorage;
      this.IndexManager = indexManager;
      this.Logger = taskExecutionContext.LoggerFactory.CreateLogger("Scanning");
    }

    /// <summary>
    /// IScanContext.
    /// </summary>
    public IScanConfiguration ScanConfiguration { get; }

    /// <summary>
    /// IScanContext.
    /// </summary>
    public IItemStorage ItemStorage { get; }

    /// <summary>
    /// IScanContext.
    /// </summary>
    public ILogger Logger { get; }

    /// <summary>
    /// IScanContext.
    /// </summary>
    public LuceneIndexManager IndexManager { get; }

    /// <summary>
    /// IScanContext.
    /// </summary>
    public void UpdateStatus(string message)
    {
      this._taskExecutionContext.UpdateStatus(message);
    }

    /// <summary>
    /// IScanContext.
    /// </summary>
    public IProgressOperation StartProgressOperation(int maxTicks)
    {
      return this._taskExecutionContext.StartProgressOperation(maxTicks);
    }
  }
}
