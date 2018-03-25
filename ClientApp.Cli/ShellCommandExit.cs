using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace Mt.MediaMan.ClientApp.Cli
{
  internal class ShellCommandExit : ShellCommandBase
  {
    protected override Task<int> OnExecuteAsync(CommandLineApplication app)
    {
      return Task.FromResult(Program.CommandExitResult);
    }
  }
}
