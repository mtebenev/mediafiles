using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace Mt.MediaFiles.ClientApp.Cli.Commands.Shell
{
  [Command("exit", Description = "Exit the shell.")]
  internal class CommandShellExit : CommandShellBase
  {
    public Task<int> OnExecuteAsync()
    {
      return Task.FromResult(Program.CommandExitResult);
    }
  }
}
