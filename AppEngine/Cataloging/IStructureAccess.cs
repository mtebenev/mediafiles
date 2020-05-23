using System.Collections.Generic;
using System.Threading.Tasks;
using Mt.MediaFiles.AppEngine.CatalogStorage;

namespace Mt.MediaFiles.AppEngine.Cataloging
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
  }
}
