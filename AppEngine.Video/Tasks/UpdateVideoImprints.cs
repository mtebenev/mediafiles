using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using AppEngine.Video.VideoImprint;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Commands;
using Mt.MediaMan.AppEngine.Tasks;

namespace Mt.MediaMan.AppEngine.Video.Tasks
{
  public interface IUpdateVideoImprintsFactory
  {
    public CatalogTaskBase Create(ICatalogItem catalogItem);
  }

  /// <summary>
  /// Updates all video imprints starting from a given catalog item.
  /// </summary>
  public class UpdateVideoImprints : CatalogTaskBase
  {
    private readonly ITaskExecutionContext _executionContext;
    private readonly IFileSystem _fileSystem;
    private readonly IVideoImprintUpdaterFactory _updaterFactory;
    private readonly ICatalogItem _catalogItem;

    /// <summary>
    /// Ctor.
    /// </summary>
    public UpdateVideoImprints(ITaskExecutionContext executionContext, IFileSystem fileSystem, IVideoImprintUpdaterFactory updaterFactory, ICatalogItem catalogItem)
    {
      this._executionContext = executionContext;
      this._fileSystem = fileSystem;
      this._updaterFactory = updaterFactory;
      this._catalogItem = catalogItem;
    }

    /// <summary>
    /// CatalogTaskBase.
    /// </summary>
    public override async Task ExecuteAsync(ICatalogContext catalogContext)
    {
      var fsPath = await CatalogItemUtils.ComposeFsPathAsync(this._catalogItem);
      using(var progressOperation = this._executionContext.ProgressIndicator.StartOperation($"Updating files at: {fsPath}"))
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
      var fsPath = await CatalogItemUtils.ComposeFsPathAsync(catalogItem);

      var extension = _fileSystem.Path.GetExtension(fsPath);
      var supportedExtensions = new[] { ".flv", ".mp4", ".wmv", ".avi", ".mkv" };
      if(supportedExtensions.Any(e => e.Equals(extension)))
      {
        progressOperation.UpdateStatus($"Updating file: {fsPath}");
        var updater = this._updaterFactory.Create();
        await updater.UpdateAsync(catalogItem, fsPath);
      }
    }
  }
}
