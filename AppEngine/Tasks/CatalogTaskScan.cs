using System.IO.Abstractions;
using System.Threading.Tasks;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.Scanning;

namespace Mt.MediaFiles.AppEngine.Tasks
{
  public interface ICatalogTaskScanFactory
  {
    ICatalogTaskScan Create(ScanParameters scanParameters);
  }

  public interface ICatalogTaskScan
  {
    Task ExecuteAsync(ICatalog catalog);
  }

  /// <summary>
  /// The scan task.
  /// </summary>
  internal sealed class CatalogTaskScan : IInternalCatalogTask, ICatalogTaskScan
  {
    private readonly ITaskExecutionContext _executionContext;
    private readonly IItemScannerFactory _scannerFactory;
    private readonly IFileSystem _fileSystem;
    private readonly IScanConfigurationBuilder _configurationBuilder;
    private readonly ScanParameters _scanParameters;

    /// <summary>
    /// Ctor.
    /// </summary>
    public CatalogTaskScan(
      ITaskExecutionContext executionContext,
      IItemScannerFactory scannerFactory,
      IFileSystem fileSystem,
      IScanConfigurationBuilder configurationBuilder,
      ScanParameters scanParameters)
    {
      this._executionContext = executionContext;
      this._scannerFactory = scannerFactory;
      this._fileSystem = fileSystem;
      this._configurationBuilder = configurationBuilder;
      this._scanParameters = scanParameters;
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
      using(var progressOperation = this._executionContext
        .ProgressIndicator
        .StartOperation($"Scanning files: {this._scanParameters.ScanPath}"))
      {
        var itemExplorer = new ItemExplorerFileSystem(this._fileSystem);
        var rootItem = catalog.RootItem;
        var mmConfig = MmConfigFactory.LoadConfig(this._scanParameters.ScanPath);
        var scanConfiguration = await this._configurationBuilder.BuildAsync(this._scanParameters, mmConfig);

        var scanner = this._scannerFactory.Create(itemExplorer, rootItem.CatalogItemId, this._scanParameters.ScanPath);

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
