using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Scanning;
using OrchardCore.FileStorage.FileSystem;

namespace Mt.MediaMan.AppEngine.Commands
{
  /// <summary>
  /// Scans files into catalog
  /// </summary>
  public class CommandScanFiles
  {
    public async Task Execute(Catalog catalog, string scanPath)
    {
      var scanQueue = new ScanQueue();
      var fileStore = new FileSystemStore(scanPath);
      var rootItem = catalog.RootItem;
      var scanContext = CreateScanContext();

      var scanner = new ItemScannerFileSystem(scanContext, fileStore, rootItem, scanQueue);
      await catalog.ScanAsync(scanner);
    }

    private IScanContext CreateScanContext()
    {
      var result = new ScanContext();
      return result;
    }
  }
}
