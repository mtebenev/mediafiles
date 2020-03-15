using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Common;
using Mt.MediaMan.AppEngine.Scanning;

namespace Mt.MediaMan.AppEngine.Cataloging
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
    /// Creates physical file system path for a given catalog item
    /// </summary>
    public static async Task<string> ComposeFsPathAsync(ICatalogItem catalogItem)
    {
      var result = "";
      var isFinished = false;

      var currentItem = catalogItem;
      do
      {
        // Determine if the item is scan root
        var infoPartScanRoot = await currentItem.GetInfoPartAsync<InfoPartScanRoot>();
        isFinished = infoPartScanRoot != null;

        if(result.Length > 0)
          result = result.Insert(0, @"\");

        if(infoPartScanRoot != null)
          result = result.Insert(0, infoPartScanRoot.RootPath);
        else
        {
          result = result.Insert(0, currentItem.Name);
          currentItem = await currentItem.GetParentItemAsync();
        }
      } while(!isFinished);

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
          catalogItem = children.FirstOrDefault(c => c.Name == pathParts[partIdx]);
          partIdx++;
        }
      }

      return catalogItem;
    }
  }
}
