using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace Mt.MediaFiles.ClientApp.Cli.Commands.Shell
{
  /// <summary>
  /// Scans new files to catalog
  /// </summary>
  [Command("cd", Description = "Change the current directory in the catalog.")]
  internal class CommandShellCd : CommandShellBase
  {
    [Argument(0, "itemNameOrId")]
    public string ItemNameOrId { get; set; }

    public async Task<int> OnExecuteAsync(IShellAppContext shellAppContext)
    {
      var item = await GetItemByNameOrIdAsync(
        shellAppContext,
        this.ItemNameOrId);

      if(item == null)
        throw new ArgumentException("Cannot load catalog item", nameof(ItemNameOrId));

      shellAppContext.CurrentItem = item;

      return Program.CommandResultContinue;
    }
  }
}
