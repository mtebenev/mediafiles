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
    private readonly IShellAppContext _shellAppContext;

    public ShellCommandLs(IShellAppContext shellAppContext)
    {
      _shellAppContext = shellAppContext;
    }

    protected override async Task<int> OnExecuteAsync()
    {
      var currentItem = _shellAppContext.CurrentItem;
      var children = await currentItem.GetChildrenAsync();

      ShellConsoleUtils.PrintItemsTable(_shellAppContext.Console, children);
      return 0;
    }
  }
}
