using System.Collections.Generic;
using Mt.MediaMan.AppEngine.CatalogStorage;

namespace Mt.MediaMan.AppEngine.Scanning
{
  /// <summary>
  /// Explores items for scanning.
  /// </summary>
  internal interface IItemExplorer
  {
    /// <summary>
    /// Returns enumerable for the item records to be saved.
    /// </summary>
    IAsyncEnumerable<CatalogItemRecord> Explore(string scanPath, int scanRootId);
  }
}
