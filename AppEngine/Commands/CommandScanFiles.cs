using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Scanning;
using OrchardCore.FileStorage.FileSystem;

namespace Mt.MediaMan.AppEngine.Commands
{
  /// <summary>
  /// Scans files into catalog
  /// </summary>
  public class CommandScanFiles
  {
    public async Task Execute(ICommandExecutionContext executionContext, string scanPath, string name)
    {
      using(var progressOperation = executionContext.ProgressIndicator.StartOperation($"Scanning files: {scanPath}"))
      {
        var scanQueue = new ScanQueue();
        var fileStore = new FileSystemStore(scanPath);
        var rootItem = executionContext.Catalog.RootItem;
        var mmConfig = MmConfigFactory.LoadConfig(scanPath);
        var scanConfiguration = new ScanConfiguration(name, mmConfig);

        var scanner = new ItemScannerFileSystem(fileStore, rootItem, scanQueue, executionContext.LoggerFactory);
        await executionContext.Catalog.ScanAsync(scanConfiguration, scanner, executionContext.LoggerFactory, progressOperation);
      }
    }
  }
}
