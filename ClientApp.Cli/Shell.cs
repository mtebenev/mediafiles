using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Commands;

namespace Mt.MediaMan.ClientApp.Cli
{
  [Command("mediaman")]
  [Subcommand("scan", typeof(ShellCommandScan))]
  [Subcommand("cls", typeof(ShellCommandCls))]
  [Subcommand("exit", typeof(ShellCommandExit))]
  internal class Shell : ShellCommandBase
  {
    public Shell()
    {
      var catalog = Catalog.CreateCatalog();

      var progressIndicator = new ProgressIndicatorConsole();
      ExecutionContext = new CommandExecutionContext(catalog, progressIndicator);
    }

    public ICommandExecutionContext ExecutionContext { get; }

    protected override Task<int> OnExecuteAsync(CommandLineApplication app)
    {
      app.ShowHelp();
      return Task.FromResult(0);
    }
  }
}
