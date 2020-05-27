using System.Collections.Generic;
using System.Threading.Tasks;
using Mt.MediaFiles.AppEngine.Cataloging;

namespace Mt.MediaFiles.AppEngine.Tools
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
    public static async Task<DuplicateFindResult> CreateAsync(ICatalog catalog, IList<int> catalogItemIds)
    {
      var catalogItems = await CatalogItemUtils.LoadItemsByIdsAsync(catalog, catalogItemIds);

      // TODOA: tuples?
      // TODOA: async select
      var fileInfos = new List<BriefFileInfo>();

      foreach(var catalogItem in catalogItems)
      {
        var brifFileInfo = new BriefFileInfo(
          catalogItem.CatalogItemId,
          catalogItem.Path,
          catalogItem.Size
          );
        fileInfos.Add(brifFileInfo);
      }

      var result = new DuplicateFindResult(fileInfos);
      return result;
    }

    public IList<BriefFileInfo> FileInfos { get; }
  }
}
