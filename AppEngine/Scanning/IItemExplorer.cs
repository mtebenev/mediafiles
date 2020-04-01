using System.Collections.Generic;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.CatalogStorage;

namespace Mt.MediaMan.AppEngine.Scanning
{
  /// <summary>
  /// Explores items for scanning.
  /// </summary>
  internal interface IItemExplorer
  {
    /// <summary>
    /// Creates the scan root info part by path.
    /// </summary>
    Task<InfoPartScanRoot> CreateScanRootPartAsync(string scanPath);

    /// <summary>
    /// Returns enumerable for the item records to be saved.
    /// </summary>
    IAsyncEnumerable<CatalogItemRecord> Explore(string scanPath, int scanRootId);
  }
}
