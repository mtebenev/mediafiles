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
    private DuplicateFindResult(IList<BriefFileInfo> fileInfos)
    {
      FileInfos = fileInfos;
    }

    /// <summary>
    /// Factory method
    /// </summary>
    public static async Task<DuplicateFindResult> Create(Catalog catalog, IList<int> catalogItemIds)
    {
      var catalogItems = await CatalogItemUtils.LoadItemsByIdsAsync(catalog, catalogItemIds);

      // TODOA: tuples?
      // TODOA: async select
      List<BriefFileInfo> fileInfos = new List<BriefFileInfo>();

      foreach(var catalogItem in catalogItems)
      {
        var fsPath = await CatalogItemUtils.ComposeFsPathAsync(catalogItem);
        var brifFileInfo = new BriefFileInfo(catalogItem.CatalogItemId, fsPath, catalogItem.Size);

        fileInfos.Add(brifFileInfo);
      }

      var result = new DuplicateFindResult(fileInfos);
      return result;
    }

    public IList<BriefFileInfo> FileInfos { get; }

    /// <summary>
    /// Detailed property differences
    /// </summary>
    public IList<PropertyDifference> PropertyDifferences { get; private set; }
  }
}
