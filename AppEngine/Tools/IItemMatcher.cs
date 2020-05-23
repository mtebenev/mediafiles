using System.Collections.Generic;
using System.Threading.Tasks;
using Mt.MediaFiles.AppEngine.Cataloging;

namespace Mt.MediaFiles.AppEngine.Tools
{
  /// <summary>
  /// Matches several items and produces detailed info about differences
  /// </summary>
  internal interface IItemMatcher
  {
    /// <summary>
    /// Matches several catalog items
    /// </summary>
    Task<IList<PropertyDifference>> MatchItemsAsync(IList<CatalogItem> catalogItems);
  }
}
