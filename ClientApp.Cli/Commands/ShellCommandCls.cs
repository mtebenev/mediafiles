using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace Mt.MediaMan.ClientApp.Cli.Commands
{
  [Command("cls", Description = "Clears screen")]
  internal class ShellCommandCls : ShellCommandBase
  {
    protected override Task<int> OnExecuteAsync(CommandLineApplication app)
    {
      Console.Clear();
      return Task.FromResult(0);
    }
  }
}
