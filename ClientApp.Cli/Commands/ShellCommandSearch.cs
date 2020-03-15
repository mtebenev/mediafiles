using System.Collections.Generic;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Commands;
using Mt.MediaMan.AppEngine.Tasks;

namespace Mt.MediaMan.ClientApp.Cli.Commands
{
  /// <summary>
  /// Searches for files in catalog
  /// </summary>
  [Command("search", Description = "Searches in catalog")]
  internal class ShellCommandSearch : ShellCommandBase
  {
    private readonly ICommandExecutionContext _executionContext;
    private readonly ShellAppContext _shellAppContext;

    public ShellCommandSearch(ICommandExecutionContext executionContext, ShellAppContext shellAppContext)
    {
      _executionContext = executionContext;
      _shellAppContext = shellAppContext;
    }

    [Argument(0, "query")]
    public string Query { get; set; }

    protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
    {
      var task = new CatalogTaskSearch(this.Query);
      var itemIds = await this._executionContext.Catalog.ExecuteTaskAsync(task);

      var items = new List<ICatalogItem>();
      foreach(int itemId in itemIds)
      {
        var item = await _executionContext.Catalog.GetItemByIdAsync(itemId);
        items.Add(item);
      }

      ShellConsoleUtils.PrintItemsTable(_shellAppContext.Console, items);

      return 0;
    }
  }
}
