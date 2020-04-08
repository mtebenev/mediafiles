using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mt.MediaFiles.AppEngine.Common;
using Mt.MediaFiles.AppEngine.Scanning;

namespace Mt.MediaFiles.AppEngine.Cataloging
{
  /// <summary>
  /// Various utilities for working with catalog
  /// </summary>
  public static class CatalogItemUtils
  {
    /// <summary>
    /// Use to load multiple catalog items at once
    /// </summary>
    public static async Task<IList<ICatalogItem>> LoadItemsByIdsAsync(ICatalog catalog, IList<int> catalogItemIds)
    {
      var result = new List<ICatalogItem>();
      foreach(var catalogItemId in catalogItemIds)
      {
        var catalogItem = await catalog.GetItemByIdAsync(catalogItemId);
        result.Add(catalogItem);
      }

      return result;
    }

    /// <summary>
    /// Searches for a catalog item by fs path. Returns null if unable to find the item.
    /// </summary>
    public static async Task<ICatalogItem> FindItemByFsPathAsync(ICatalog catalog, string fsPath)
    {
      InfoPartScanRoot infoPartScanRoot = null;

      // Find root
      var rootChildren = await catalog.RootItem.GetChildrenAsync();
      ICatalogItem catalogItem = null;
      foreach(var item in rootChildren)
      {
        var rootInfoPart = await item.GetInfoPartAsync<InfoPartScanRoot>();
        if(PathUtils.IsBaseOfPath(fsPath, rootInfoPart.RootPath))
        {
          catalogItem = item;
          infoPartScanRoot = rootInfoPart;
          break;
        }
      }

      if(infoPartScanRoot != null)
      {
        var pathParts = PathUtils.GetRelativeParts(fsPath, infoPartScanRoot.RootPath);
        var partIdx = 0;
        while(catalogItem != null && partIdx < pathParts.Length)
        {
          var children = await catalogItem.GetChildrenAsync();
          catalogItem = children.FirstOrDefault(c => c.Path == pathParts[partIdx]);
          partIdx++;
        }
      }

      return catalogItem;
    }
  }
}
