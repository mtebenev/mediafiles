using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace Mt.MediaMan.ClientApp.Cli.Commands
{
  /// <summary>
  /// Scans new files to catalog
  /// </summary>
  [Command("cd", Description = "Changes current directory")]
  internal class ShellCommandCd : ShellCommandBase
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
