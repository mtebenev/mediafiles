using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using Mt.MediaFiles.AppEngine.Cataloging;

namespace Mt.MediaFiles.AppEngine.Tasks
{
  public interface ICatalogTaskCheckStatusFactory
  {
    CatalogTaskBase<IList<CheckStatusResult>> Create(ICatalogItem catalogItem);
  }

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
  public class CatalogTaskCheckStatus : CatalogTaskBase<IList<CheckStatusResult>>
  {
    private readonly IFileSystem _fileSystem;
    private readonly ICatalogItem _catalogItem;

    public CatalogTaskCheckStatus(IFileSystem fileSystem, ICatalogItem catalogItem)
    {
      this._fileSystem = fileSystem;
      this._catalogItem = catalogItem;
    }

    /// <summary>
    /// CatalogTaskBase.
    /// </summary>
    protected override async Task<IList<CheckStatusResult>> ExecuteAsync(ICatalogContext catalogContext)
    {
      var result = new List<CheckStatusResult>();

      // Enumerate
      var walker = CatalogTreeWalker.CreateDefaultWalker(catalogContext.Catalog, this._catalogItem.CatalogItemId);
      result = await walker
        .Where(ci => !ci.IsDirectory)
        .SelectAwait(async ci =>
        {
          var itemResult = await this.CreateItemResult(ci);
          return itemResult;
        })
        .ToListAsync();

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
        Path = catalogItem.Path
      };

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
