using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace Mt.MediaMan.ClientApp.Cli
{
  [Command("exit", Description = "Scans files to catalog")]
  internal class ShellCommandExit : ShellCommandBase
  {
    protected override Task<int> OnExecuteAsync(CommandLineApplication app)
    {
      return Task.FromResult(Program.CommandExitResult);
    }
  }
}
