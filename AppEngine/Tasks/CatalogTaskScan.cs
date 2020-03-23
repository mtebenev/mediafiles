using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Commands;
using Mt.MediaMan.AppEngine.Scanning;
using OrchardCore.FileStorage.FileSystem;

namespace Mt.MediaMan.AppEngine.Tasks
{
  public interface ICatalogTaskScanFactory
  {
    ICatalogTaskScan Create(string scanPath, string name);
  }

  public interface ICatalogTaskScan
  {
    Task ExecuteAsync(ICatalog catalog);
  }

  /// <summary>
  /// The scan task.
  /// </summary>
  public sealed class CatalogTaskScan : IInternalCatalogTask, ICatalogTaskScan
  {
    private readonly ITaskExecutionContext _executionContext;
    private readonly string _scanPath;
    private readonly string _name;

    /// <summary>
    /// Ctor.
    /// </summary>
    public CatalogTaskScan(ITaskExecutionContext executionContext, string scanPath, string name)
    {
      this._executionContext = executionContext;
      this._scanPath = scanPath;
      this._name = name;
    }

    public Task ExecuteAsync(ICatalog catalog)
    {
      return catalog.ExecuteTaskAsync(this);
    }

    /// <summary>
    /// IInternalCatalogTask
    /// </summary>
    async Task IInternalCatalogTask.ExecuteAsync(Catalog catalog)
    {
      using(var progressOperation = this._executionContext.ProgressIndicator.StartOperation($"Scanning files: {this._scanPath}"))
      {
        var scanQueue = new ScanQueue();
        var fileStore = new FileSystemStore(this._scanPath);
        var rootItem = catalog.RootItem;
        var mmConfig = MmConfigFactory.LoadConfig(this._scanPath);
        var scanConfiguration = new ScanConfiguration(this._name, mmConfig, this._executionContext.ServiceProvider);

        var scanner = new ItemScannerFileSystem(fileStore, rootItem, scanQueue, this._executionContext.LoggerFactory);

        // Create scan context and execute
        var scanContext = new ScanContext(
          scanConfiguration,
          catalog.ItemStorage,
          catalog.IndexManager,
          this._executionContext.LoggerFactory,
          progressOperation);

        await scanner.Scan(scanContext);
      }
    }
  }
}
