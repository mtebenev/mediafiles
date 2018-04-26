using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace Mt.MediaMan.ClientApp.Cli
{
  [Command("mediaman")]
  [Subcommand("init-catalog", typeof(ShellCommandInitCatalog))]
  [Subcommand("ls", typeof(ShellCommandLs))]
  [Subcommand("cd", typeof(ShellCommandCd))]
  [Subcommand("scan", typeof(ShellCommandScan))]
  [Subcommand("cls", typeof(ShellCommandCls))]
  [Subcommand("exit", typeof(ShellCommandExit))]
  [Subcommand("get-info", typeof(ShellCommandGetInfo))]
  [Subcommand("search", typeof(ShellCommandSearch))]
  internal class Shell : ShellCommandBase
  {
    protected override Task<int> OnExecuteAsync(CommandLineApplication app)
    {
      app.ShowHelp();
      return Task.FromResult(0);
    }
  }
}
