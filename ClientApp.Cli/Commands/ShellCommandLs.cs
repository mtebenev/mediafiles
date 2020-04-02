using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace Mt.MediaMan.ClientApp.Cli.Commands
{
  /// <summary>
  /// Scans new files to catalog
  /// </summary>
  [Command("ls", Description = "Prints content of the current folder")]
  internal class ShellCommandLs : ShellCommandBase
  {
    public async Task<int> OnExecuteAsync(IShellAppContext shellAppContext)
    {
      var currentItem = shellAppContext.CurrentItem;
      var children = await currentItem.GetChildrenAsync();

      ShellConsoleUtils.PrintItemsTable(shellAppContext.Console, children);
      return Program.CommandResultContinue;
    }
  }
}
