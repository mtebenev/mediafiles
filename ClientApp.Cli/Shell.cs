using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Commands;

namespace Mt.MediaMan.ClientApp.Cli
{
  [Command("mediaman")]
  [Subcommand("init-catalog", typeof(ShellCommandInitCatalog))]
  [Subcommand("scan", typeof(ShellCommandScan))]
  [Subcommand("cls", typeof(ShellCommandCls))]
  [Subcommand("exit", typeof(ShellCommandExit))]
  internal class Shell : ShellCommandBase, IDisposable
  {
    private readonly Catalog _catalog;

    public Shell()
    {
      var connectionString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=mediaman;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
      _catalog = Catalog.CreateCatalog(connectionString);

      var progressIndicator = new ProgressIndicatorConsole();
      ExecutionContext = new CommandExecutionContext(_catalog, progressIndicator);
    }

    public ICommandExecutionContext ExecutionContext { get; }

    /// <summary>
    /// IDisposable
    /// </summary>
    public void Dispose()
    {
      _catalog?.Dispose();
    }

    protected override Task<int> OnExecuteAsync(CommandLineApplication app)
    {
      app.ShowHelp();
      return Task.FromResult(0);
    }

    /// <summary>
    /// TODO: remove this. Use dependency injection and open catalog on startup
    /// </summary>
    public Task InitializeAsync()
    {
      return ExecutionContext.Catalog.OpenAsync();
    }
  }
}
