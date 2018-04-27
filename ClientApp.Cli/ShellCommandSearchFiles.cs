using System.Collections.Generic;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Commands;

namespace Mt.MediaMan.ClientApp.Cli
{
  /// <summary>
  /// Scans new files to catalog
  /// </summary>
  [Command("search-files", Description = "Searches for files in catalog")]
  internal class ShellCommandSearchFiles : ShellCommandBase
  {
    private readonly ICommandExecutionContext _executionContext;
    private readonly ShellAppContext _shellAppContext;

    public ShellCommandSearchFiles(ICommandExecutionContext executionContext, ShellAppContext shellAppContext)
    {
      _executionContext = executionContext;
      _shellAppContext = shellAppContext;
    }

    [Argument(0, "query")]
    public string Query { get; set; }

    protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
    {
      var command = new CommandSearchFiles();
      var itemIds = await command.ExecuteAsync(_executionContext.Catalog, Query);

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
