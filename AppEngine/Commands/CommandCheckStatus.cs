using System.Collections.Generic;
using System.IO.Abstractions;
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
    private readonly IFileSystem _fileSystem;

    public CommandCheckStatus(IFileSystem fileSystem)
    {
      this._fileSystem = fileSystem;
    }

    public async Task<IList<CheckStatusResult>> ExecuteAsync(ICatalog catalog, string fsPath)
    {
      var result = new List<CheckStatusResult>();
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

      // Enumerate
      if(catalogItem != null)
      {
        await this.CheckStatusAsync(catalog, catalogItem, result);
      }

      return result;
    }

    /// <summary>
    /// Performs checking starting from the catalog item.
    /// </summary>
    private async Task CheckStatusAsync(ICatalog catalog, ICatalogItem checkRootItem, List<CheckStatusResult> result)
    {
      var walker = CatalogTreeWalker.CreateDefaultWalker(catalog, checkRootItem.CatalogItemId);
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

      if(!this._fileSystem.File.Exists(result.Path))
      {
        result.Status = FsItemStatus.Deleted;
      }
      else
      {
        var fileInfo = this._fileSystem.FileInfo.FromFileName(result.Path);
        result.Status = fileInfo.Length == catalogItem.Size ? FsItemStatus.Ok : FsItemStatus.Changed;
      }

      return result;
    }
  }
}
