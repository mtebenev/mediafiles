using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Commands;

namespace AppEngine.Video.VideoImprint
{
  /// <summary>
  /// Updates items in the catalog starting from the folder.
  /// Note: This command should be moved to the app engine as soon as it become generic.
  /// The problem is that AppEngine.Video depends on the AppEngine and the AppEngine cannot simply create the tasks.
  /// Use some kind of DI for that?
  /// </summary>
  public class CommandUpdate
  {
    private readonly IFileSystem _fileSystem;
    private readonly IVideoImprintStorage _videoImprintStorage;

    public CommandUpdate(IFileSystem fileSystem, IVideoImprintStorage videoImprintStorage)
    {
      this._fileSystem = fileSystem;
      this._videoImprintStorage = videoImprintStorage;
    }

    public async Task ExecuteAsync(ICommandExecutionContext executionContext, string fsPath)
    {
      var catalogItem = await CatalogItemUtils.FindItemByFsPathAsync(executionContext.Catalog, fsPath);
      if(catalogItem != null)
      {
        using(var progressOperation = executionContext.ProgressIndicator.StartOperation($"Updating files at: {fsPath}"))
        {
          var walker = CatalogTreeWalker.CreateDefaultWalker(executionContext.Catalog, catalogItem.CatalogItemId);
          await walker.ForEachAwaitAsync(async ci =>
          {
            await this.UpdateItem(progressOperation, ci);
          });

          progressOperation.UpdateStatus("Done.");
        }
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
        var task = new UpdateVideoImprintTask(this._videoImprintStorage, catalogItem, fsPath);
        await task.ExecuteAsync();
      }
    }
  }
}
