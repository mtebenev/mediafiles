using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace Mt.MediaFiles.ClientApp.Cli.Commands
{
  [Command("cls", Description = "Clear the console history.")]
  internal class ShellCommandCls : ShellCommandBase
  {
    public Task<int> OnExecuteAsync()
    {
      Console.Clear();
      return Task.FromResult(Program.CommandResultContinue);
    }
  }
}
