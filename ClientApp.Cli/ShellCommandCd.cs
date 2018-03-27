using System;
using System.Linq;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Commands;

namespace Mt.MediaMan.ClientApp.Cli
{
  /// <summary>
  /// Scans new files to catalog
  /// </summary>
  [Command("cd", Description = "Changes current directory")]
  internal class ShellCommandCd : ShellCommandBase
  {
    private readonly ICommandExecutionContext _executionContext;
    private readonly ShellContext _shellContext;

    public ShellCommandCd(ICommandExecutionContext executionContext, ShellContext shellContext)
    {
      _executionContext = executionContext;
      _shellContext = shellContext;
    }

    [Argument(0, "itemName")]
    public string ItemName { get; set; }

    protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
    {
      var currentItem = _shellContext.CurrentItem;
      var children = await currentItem.GetChildrenAsync();

      ICatalogItem child = null;

      // Try find by ID
      if(ItemName.StartsWith(':'))
      {
        var itemId = int.Parse(ItemName.Substring(1));
        child = await _executionContext.Catalog.GetItemByIdAsync(itemId);
      }
      else // by name among children
      {
        child = children
          .FirstOrDefault(c => c.Name.Equals(ItemName, StringComparison.InvariantCultureIgnoreCase));
      }

      if(child != null)
        _shellContext.CurrentItem = child;

      return 0;
    }
  }
}
