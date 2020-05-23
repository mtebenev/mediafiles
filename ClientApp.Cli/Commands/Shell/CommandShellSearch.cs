using System.Collections.Generic;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.Tasks;

namespace Mt.MediaFiles.ClientApp.Cli.Commands.Shell
{
  /// <summary>
  /// Searches for files in catalog
  /// </summary>
  [Command("search", Description = "Search for files in the catalog.")]
  internal class CommandShellSearch : CommandShellBase
  {
    private readonly IShellAppContext _shellAppContext;

    public CommandShellSearch(IShellAppContext shellAppContext)
    {
      _shellAppContext = shellAppContext;
    }

    [Argument(0, "query")]
    public string Query { get; set; }

    public async Task<int> OnExecuteAsync()
    {
      var task = new CatalogTaskSearch(this.Query);
      var itemIds = await task.ExecuteAsync(this._shellAppContext.Catalog);

      var items = new List<ICatalogItem>();
      foreach(var itemId in itemIds)
      {
        var item = await this._shellAppContext.Catalog.GetItemByIdAsync(itemId);
        items.Add(item);
      }

      ShellConsoleUtils.PrintItemsTable(_shellAppContext.Console, items);

      return Program.CommandResultContinue;
    }
  }
}
