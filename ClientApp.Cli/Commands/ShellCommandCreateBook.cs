using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaMan.AppEngine.Commands;
using Mt.MediaMan.AppEngine.Ebooks.Commands;

namespace Mt.MediaMan.ClientApp.Cli.Commands
{
  /// <summary>
  /// Creates a single ebook record
  /// </summary>
  [Command("create-book", Description = "Creates eBook record for a catalog item")]
  internal class ShellCommandCreateBook : ShellCommandBase
  {
    private readonly ICommandExecutionContext _executionContext;
    private readonly ShellAppContext _shellAppContext;

    public ShellCommandCreateBook(ICommandExecutionContext executionContext, ShellAppContext shellAppContext)
    {
      _executionContext = executionContext;
      _shellAppContext = shellAppContext;
    }

    [Argument(0, "itemNameOrId")]
    public string ItemNameOrId { get; set; }

    protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
    {
      var item = await GetItemByNameOrIdAsync(_shellAppContext, _executionContext, ItemNameOrId);

      if(item == null)
        throw new ArgumentException("Cannot load catalog item", nameof(ItemNameOrId));

      var command = new CommandCreateEbook();
      await command.Execute(_executionContext.Catalog, item.CatalogItemId);

      return 0;
    }
  }
}
