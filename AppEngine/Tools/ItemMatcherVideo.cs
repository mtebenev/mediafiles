using System.Collections.Generic;
using System.Threading.Tasks;
using Mt.MediaFiles.AppEngine.Cataloging;

namespace Mt.MediaFiles.AppEngine.Tools
{
  /// <summary>
  /// Item matcher for video info
  /// </summary>
  internal class ItemMatcherVideo : IItemMatcher
  {
    public Task<IList<PropertyDifference>> MatchItemsAsync(IList<CatalogItem> catalogItems)
    {
      throw new System.NotImplementedException();
    }
  }
}
