using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Cataloging;

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
      var catalogItem = await CatalogItemUtils.FindItemByFsPathAsync(catalog, fsPath);

      // Enumerate
      if(catalogItem != null)
      {
        var walker = CatalogTreeWalker.CreateDefaultWalker(catalog, catalogItem.CatalogItemId);
        result = await walker
          .Where(ci => !ci.IsDirectory)
          .SelectAwait(async ci =>
          {
            var itemResult = await this.CreateItemResult(ci);
            return itemResult;
          })
          .ToListAsync();
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
