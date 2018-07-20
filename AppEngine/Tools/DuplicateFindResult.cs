using System.Collections.Generic;

namespace Mt.MediaMan.AppEngine.Tools
{
  /// <summary>
  /// Contains result of duplicates search
  /// </summary>
  public class DuplicateFindResult
  {
    public DuplicateFindResult(IList<int> catalogItemIds)
    {
      CatalogItemIds = catalogItemIds;
    }

    public IList<int> CatalogItemIds { get; }
  }
}
