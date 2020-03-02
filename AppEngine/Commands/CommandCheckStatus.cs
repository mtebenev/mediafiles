using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Common;
using Mt.MediaMan.AppEngine.Scanning;

namespace Mt.MediaMan.AppEngine.Commands
{
  public enum FsItemStatus
  {
    /// <summary>
    /// The actual file matches to the cataloged one
    /// </summary>
    Ok,

    /// <summary>
    /// The actual file has been removed.
    /// </summary>
    Deleted,

    /// <summary>
    /// The actual file has been changed.
    /// </summary>
    Changed
  }

  /// <summary>
  /// Status result for a single item.
  /// </summary>
  public class CheckStatusResult
  {
    public int CatalogItemId { get; set; }
    public string Path { get; set; }
    public FsItemStatus Status { get; set; }
  }

  /// <summary>
  /// Checks status of actual FS files compared to catalog.
  /// </summary>
  public class CommandCheckStatus
  {
    public async Task<IList<CheckStatusResult>> ExecuteAsync(ICatalog catalog, string fsPath)
    {
      var partsEnumerator = new PathPartsEnumerator(fsPath);

      // Find root
      var rootChildren = await catalog.RootItem.GetChildrenAsync();
      ICatalogItem catalogItem = null;
      foreach(var item in rootChildren)
      {
        var rootInfoPart = await item.GetInfoPartAsync<InfoPartScanRoot>();
        if(rootInfoPart.RootPath == partsEnumerator.Root)
        {
          catalogItem = item;
          break;
        }
      }

      var partIdx = 0;
      while(catalogItem != null && partIdx < partsEnumerator.Parts.Length)
      {
        var children = await catalogItem.GetChildrenAsync();
        catalogItem = children.FirstOrDefault(c => c.Name == partsEnumerator.Parts[partIdx]);
        partIdx++;
      }

      // Enumerate
      var result = new List<CheckStatusResult>();
      if(catalogItem != null)
      {
        var walker = CatalogTreeWalker.CreateDefaultWalker(catalog, catalogItem.CatalogItemId);
        var catalogItems = await walker.ToList();
        foreach(var ci in catalogItems)
        {
          if(!ci.IsDirectory)
          {
            var itemResult = await this.CreateItemResult(ci);
            result.Add(itemResult);
          }
        }
      }

      return result;
    }

    /// <summary>
    /// Creates a result for a single item.
    /// </summary>
    private async Task<CheckStatusResult> CreateItemResult(ICatalogItem catalogItem)
    {
      var result = new CheckStatusResult
      {
        CatalogItemId = catalogItem.CatalogItemId,
      };
      result.Path = await CatalogItemUtils.ComposeFsPathAsync(catalogItem);

      if(!File.Exists(result.Path))
      {
        result.Status = FsItemStatus.Deleted;
      }
      else
      {
        var fileInfo = new FileInfo(result.Path);
        result.Status = fileInfo.Length == catalogItem.Size ? FsItemStatus.Ok : FsItemStatus.Changed;
      }

      return result;
    }
  }
}
