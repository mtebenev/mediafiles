using System;
using System.Linq;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.ClientApp.Cli.Commands.Shell;

namespace Mt.MediaFiles.ClientApp.Cli.Commands
{
  [HelpOption("--help")]
  internal abstract class CommandShellBase
  {
    /// <summary>
    /// Injected shell command instance instance.
    /// </summary>
    public ICommandShell Parent { get; set; }


    /// <summary>
    /// Use to obtain the shell command context.
    /// </summary>
    protected IShellAppContext ShellAppContext => this.Parent.ShellAppContext;

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
          .FirstOrDefault(c => c.Path.Equals(itemNameOrId, StringComparison.InvariantCultureIgnoreCase));
      }

      return child;
    }
  }
}
