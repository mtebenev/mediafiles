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
  internal class Shell : ShellCommandBase, IDisposable
  {
    private readonly Catalog _catalog;
    private ICommandExecutionContext _executionContext;

    public Shell()
    {
      var connectionString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=mediaman;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
      _catalog = Catalog.CreateCatalog(connectionString);
    }

    public ICommandExecutionContext ExecutionContext
    {
      get
      {
        if(_executionContext == null)
          throw new InvalidOperationException("Execution context is not initialized");

        return _executionContext;
      }
    }

    public ICatalogItem CurrentItem { get; set; }

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
    public async Task InitializeAsync()
    {
      if(!_catalog.IsOpen)
      {
        await _catalog.OpenAsync();
        var progressIndicator = new ProgressIndicatorConsole();
        _executionContext = new CommandExecutionContext(_catalog, progressIndicator);
        CurrentItem = _catalog.RootItem;
      }
    }
  }
}
