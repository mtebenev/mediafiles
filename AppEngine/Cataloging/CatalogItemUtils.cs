using System.Collections.Generic;
using System.Threading.Tasks;
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
    public static async Task<IList<ICatalogItem>> LoadItemsByIdsAsync(Catalog catalog, IList<int> catalogItemIds)
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
  }
}
