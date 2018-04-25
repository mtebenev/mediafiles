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
      var scanConfiguration = new ScanConfiguration();

      var scanner = new ItemScannerFileSystem(fileStore, rootItem, scanQueue);
      await catalog.ScanAsync(scanConfiguration, scanner);
    }
  }
}
