using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.Tasks;
using Mt.MediaFiles.AppEngine.Video.VideoImprint;

namespace Mt.MediaFiles.AppEngine.Video.Tasks
{
  public interface ICatalogTaskUpdateVideoImprintsFactory
  {
    public CatalogTaskBase Create(ICatalogItem catalogItem);
  }

  /// <summary>
  /// Updates all video imprints starting from a given catalog item.
  /// </summary>
  public class CatalogTaskUpdateVideoImprints : CatalogTaskBase
  {
    private readonly ITaskExecutionContext _executionContext;
    private readonly IFileSystem _fileSystem;
    private readonly IVideoImprintUpdaterFactory _updaterFactory;
    private readonly ICatalogItem _catalogItem;

    /// <summary>
    /// Ctor.
    /// </summary>
    public CatalogTaskUpdateVideoImprints(ITaskExecutionContext executionContext, IFileSystem fileSystem, IVideoImprintUpdaterFactory updaterFactory, ICatalogItem catalogItem)
    {
      this._executionContext = executionContext;
      this._fileSystem = fileSystem;
      this._updaterFactory = updaterFactory;
      this._catalogItem = catalogItem;
    }

    /// <summary>
    /// CatalogTaskBase.
    /// </summary>
    protected override async Task ExecuteAsync(ICatalogContext catalogContext)
    {
      using(var progressOperation = this._executionContext.ProgressIndicator.StartOperation($"Updating files at: {this._catalogItem.Path}"))
      {
        var walker = CatalogTreeWalker.CreateDefaultWalker(catalogContext.Catalog, this._catalogItem.CatalogItemId);
        await walker.ForEachAwaitAsync(async ci =>
        {
          await this.UpdateItem(progressOperation, ci);
        });

        progressOperation.UpdateStatus("Done.");
      }
    }

    /// <summary>
    /// Updates a single item.
    /// </summary>
    private async Task UpdateItem(IProgressOperation progressOperation, ICatalogItem catalogItem)
    {
      var extension = _fileSystem.Path.GetExtension(catalogItem.Path);
      var supportedExtensions = new[] { ".flv", ".mp4", ".wmv", ".avi", ".mkv" };
      if(supportedExtensions.Any(e => e.Equals(extension)))
      {
        progressOperation.UpdateStatus($"Updating file: {catalogItem.Path}");
        var updater = this._updaterFactory.Create();
        await updater.UpdateAsync(catalogItem, catalogItem.Path);
      }
    }
  }
}
