using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace Mt.MediaMan.ClientApp.Cli
{
  [Command("mediaman")]
  [Subcommand("reset-catalog", typeof(ShellCommandResetCatalog))]
  [Subcommand("ls", typeof(ShellCommandLs))]
  [Subcommand("cd", typeof(ShellCommandCd))]
  [Subcommand("scan", typeof(ShellCommandScan))]
  [Subcommand("cls", typeof(ShellCommandCls))]
  [Subcommand("exit", typeof(ShellCommandExit))]
  [Subcommand("get-info", typeof(ShellCommandGetInfo))]
  [Subcommand("search", typeof(ShellCommandSearch))]
  [Subcommand("search-files", typeof(ShellCommandSearchFiles))]
  [Subcommand("create-book", typeof(ShellCommandCreateBook))]
  [Subcommand("search-book", typeof(ShellCommandSearchBook))]
  [Subcommand("find-duplicates", typeof(ShellCommandFindDuplicates))]
  internal class Shell : ShellCommandBase
  {
    protected override Task<int> OnExecuteAsync(CommandLineApplication app)
    {
      app.ShowHelp();
      return Task.FromResult(0);
    }
  }
}
