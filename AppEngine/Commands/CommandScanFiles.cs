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
    public async Task Execute(Cataloging.Catalog catalog, string scanPath)
    {
      var scanQueue = new ScanQueue();
      var fileStore = new FileSystemStore(scanPath);

      var scanner = new ItemScannerFileSystem(fileStore, scanQueue);
      await catalog.ScanAsync(scanner);
    }
  }
}
