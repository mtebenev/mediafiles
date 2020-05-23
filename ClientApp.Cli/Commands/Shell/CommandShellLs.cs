using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace Mt.MediaFiles.ClientApp.Cli.Commands.Shell
{
  /// <summary>
  /// Scans new files to catalog
  /// </summary>
  [Command("ls", Description = "Print the content of the current directory in the catalog.")]
  internal class CommandShellLs : CommandShellBase
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
