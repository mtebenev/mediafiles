using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaFiles.ClientApp.Cli;

namespace Mt.MediaFiles.ClientApp.Cli.Commands
{
  [Command("cls", Description = "Clears screen")]
  internal class ShellCommandCls : ShellCommandBase
  {
    public Task<int> OnExecuteAsync()
    {
      Console.Clear();
      return Task.FromResult(Program.CommandResultContinue);
    }
  }
}
