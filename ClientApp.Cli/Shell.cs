using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Commands;

namespace Mt.MediaMan.ClientApp.Cli
{
  [Command("mediaman")]
  [Subcommand("init-catalog", typeof(ShellCommandInitCatalog))]
  [Subcommand("ls", typeof(ShellCommandLs))]
  [Subcommand("cd", typeof(ShellCommandCd))]
  [Subcommand("scan", typeof(ShellCommandScan))]
  [Subcommand("cls", typeof(ShellCommandCls))]
  [Subcommand("exit", typeof(ShellCommandExit))]
  internal class Shell : ShellCommandBase
  {
    private readonly ICommandExecutionContext _executionContext;

    public Shell(ICommandExecutionContext executionContext)
    {
      _executionContext = executionContext;
      CurrentItem = _executionContext.Catalog.RootItem;
    }

    public ICommandExecutionContext ExecutionContext => _executionContext;

    /// <summary>
    /// Get/set current item for navigation
    /// </summary>
    public ICatalogItem CurrentItem { get; set; }

    protected override Task<int> OnExecuteAsync(CommandLineApplication app)
    {
      app.ShowHelp();
      return Task.FromResult(0);
    }
  }
}
