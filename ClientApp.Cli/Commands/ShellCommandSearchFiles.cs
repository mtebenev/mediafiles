using System.Collections.Generic;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Commands;
using Mt.MediaMan.AppEngine.Tasks;

namespace Mt.MediaMan.ClientApp.Cli.Commands
{
  /// <summary>
  /// Scans new files to catalog
  /// </summary>
  [Command("search-files", Description = "Searches for files in catalog")]
  internal class ShellCommandSearchFiles : ShellCommandBase
  {
    private readonly ITaskExecutionContext _executionContext;
    private readonly ShellAppContext _shellAppContext;

    public ShellCommandSearchFiles(ITaskExecutionContext executionContext, ShellAppContext shellAppContext)
    {
      _executionContext = executionContext;
      _shellAppContext = shellAppContext;
    }

    [Argument(0, "query")]
    public string Query { get; set; }

    protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
    {
      var task = new CatalogTaskSearchFiles(this.Query);
      var itemIds = await task.ExecuteAsync(this._shellAppContext.Catalog);

      var items = new List<ICatalogItem>();
      foreach(int itemId in itemIds)
      {
        var item = await this._shellAppContext.Catalog.GetItemByIdAsync(itemId);
        items.Add(item);
      }

      ShellConsoleUtils.PrintItemsTable(_shellAppContext.Console, items);

      return Program.CommandResultContinue;
    }
  }
}
