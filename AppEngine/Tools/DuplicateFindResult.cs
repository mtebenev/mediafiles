using System.Collections.Generic;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Cataloging;

namespace Mt.MediaMan.AppEngine.Tools
{
  /// <summary>
  /// Contains result of duplicates search
  /// </summary>
  public class DuplicateFindResult
  {
    private DuplicateFindResult(IList<int> catalogItemIds, List<string> filePaths)
    {
      CatalogItemIds = catalogItemIds;
      FilePaths = filePaths;
    }

    /// <summary>
    /// Factory method
    /// </summary>
    public static async Task<DuplicateFindResult> Create(Catalog catalog, IList<int> catalogItemIds)
    {
      var catalogItems = await CatalogItemUtils.LoadItemsByIdsAsync(catalog, catalogItemIds);

      // TODOA: tuples?
      // TODOA: async select
      List<int> itemIds = new List<int>();
      List<string> paths = new List<string>();

      foreach(var catalogItem in catalogItems)
      {
        var fsPath = await CatalogItemUtils.ComposeFsPathAsync(catalogItem);

        itemIds.Add(catalogItem.CatalogItemId);
        paths.Add(fsPath);
      }

      var result = new DuplicateFindResult(itemIds, paths);
      return result;
    }

    public IList<int> CatalogItemIds { get; }

    /// <summary>
    /// Duplicate file paths
    /// </summary>
    public IList<string> FilePaths { get; }

    /// <summary>
    /// Detailed property differences
    /// </summary>
    public IList<PropertyDifference> PropertyDifferences { get; private set; }
  }
}
