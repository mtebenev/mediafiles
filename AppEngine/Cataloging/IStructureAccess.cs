using System.Collections.Generic;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.CatalogStorage;

namespace Mt.MediaMan.AppEngine.Cataloging
{
  /// <summary>
  /// Provides access to catalog items with respect to FS structure.
  /// </summary>
  internal interface IStructureAccess
  {
    /// <summary>
    /// Returns direct children at the item location.
    /// </summary>
    Task<IList<CatalogItemRecord>> QueryLevelAsync(CatalogItemLocation catalogItemLocation);

    /// <summary>
    /// Creates location object from ID of an item belonging to the scan root.
    /// </summary>
    Task<CatalogItemLocation> CreateLocationAsync(int catalogItemId);
  }
}
