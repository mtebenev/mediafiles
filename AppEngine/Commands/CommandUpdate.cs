using System.Linq;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Cataloging;

namespace Mt.MediaMan.AppEngine.Commands
{
  /// <summary>
  /// Updates items in the catalog starting from the folder.
  /// </summary>
  public class CommandUpdate
  {
    public CommandUpdate()
    {
    }

    public async Task ExecuteAsync(ICommandExecutionContext executionContext, string fsPath)
    {
      var catalogItem = await CatalogItemUtils.FindItemByFsPathAsync(executionContext.Catalog, fsPath);
      if(catalogItem != null)
      {
        using(var progressOperation = executionContext.ProgressIndicator.StartOperation($"Updating files at: {fsPath}"))
        {
          var walker = CatalogTreeWalker.CreateDefaultWalker(executionContext.Catalog, catalogItem.CatalogItemId);
          var catalogItems = await walker.ToList();
          foreach(var ci in catalogItems)
          {
            await this.UpdateItem(progressOperation, ci);
          }

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
      progressOperation.UpdateStatus($"Updating file: {fsPath}");
    }
  }
}
