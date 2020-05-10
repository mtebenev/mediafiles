using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace Mt.MediaFiles.ClientApp.Cli.Commands
{
  [Command("exit", Description = "Exit the shell.")]
  internal class ShellCommandExit : ShellCommandBase
  {
    public Task<int> OnExecuteAsync()
    {
      return Task.FromResult(Program.CommandExitResult);
    }
  }
}
