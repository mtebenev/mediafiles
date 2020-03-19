using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace Mt.MediaMan.ClientApp.Cli.Commands
{
  [Command("exit", Description = "Scans files to catalog")]
  internal class ShellCommandExit : ShellCommandBase
  {
    protected override Task<int> OnExecuteAsync()
    {
      return Task.FromResult(Program.CommandExitResult);
    }
  }
}
