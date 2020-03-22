using System;
using System.Linq;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaMan.AppEngine.Cataloging;

namespace Mt.MediaMan.ClientApp.Cli.Commands
{
  [HelpOption("--help")]
  internal abstract class ShellCommandBase
  {
    /// <summary>
    /// Load a catalog item by name (among current children) or ID (if parameter starts with ':')
    /// </summary>
    protected async Task<ICatalogItem> GetItemByNameOrIdAsync(IShellAppContext shellContext, string itemNameOrId)
    {
      var currentItem = shellContext.CurrentItem;

      ICatalogItem child = null;

      // Try find by ID
      if(itemNameOrId.StartsWith(':'))
      {
        var itemId = int.Parse(itemNameOrId.Substring(1));
        child = await shellContext.Catalog.GetItemByIdAsync(itemId);
      }
      else // by name among children
      {
        var children = await currentItem.GetChildrenAsync();
        child = children
          .FirstOrDefault(c => c.Name.Equals(itemNameOrId, StringComparison.InvariantCultureIgnoreCase));
      }

      return child;
    }
  }
}
