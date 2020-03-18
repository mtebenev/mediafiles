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
    private readonly IShellAppContext _shellAppContext;

    public ShellCommandCd(IShellAppContext shellAppContext)
    {
      _shellAppContext = shellAppContext;
    }

    [Argument(0, "itemNameOrId")]
    public string ItemNameOrId { get; set; }

    protected override async Task<int> OnExecuteAsync()
    {
      var item = await GetItemByNameOrIdAsync(
        this._shellAppContext,
        this.ItemNameOrId);

      if(item == null)
        throw new ArgumentException("Cannot load catalog item", nameof(ItemNameOrId));

      _shellAppContext.CurrentItem = item;

      return Program.CommandResultContinue;
    }
  }
}
