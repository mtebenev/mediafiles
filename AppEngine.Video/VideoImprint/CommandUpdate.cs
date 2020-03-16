using System;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Commands;

namespace AppEngine.Video.VideoImprint
{
  /// <summary>
  /// Updates items in the catalog starting from the folder.
  /// Note: This command should be moved to the app engine as soon as it become generic.
  /// </summary>
  public class CommandUpdate
  {
    private readonly IFileSystem _fileSystem;
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Ctor.
    /// </summary>
    public CommandUpdate(IFileSystem fileSystem, IServiceProvider serviceProvider)
    {
      this._fileSystem = fileSystem;
      this._serviceProvider = serviceProvider;
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
        var task = ActivatorUtilities.CreateInstance<UpdateVideoImprintTask>(this._serviceProvider , catalogItem, fsPath);
        await task.ExecuteAsync();
      }
    }
  }
}
