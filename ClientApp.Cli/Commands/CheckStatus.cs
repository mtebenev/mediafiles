using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaMan.AppEngine.Commands;

namespace Mt.MediaMan.ClientApp.Cli.Commands
{
  /// <summary>
  /// Checks the status of the files in the current directory.
  /// </summary>
  [Command("status", Description = "Checks the status of the files in the current directory")]
  internal class CheckStatus
  {
    private readonly ICommandExecutionContext _executionContext;
    private readonly ShellAppContext _shellAppContext;

    public CheckStatus(ICommandExecutionContext executionContext, ShellAppContext shellAppContext)
    {
      _executionContext = executionContext;
      _shellAppContext = shellAppContext;
    }

    public async Task<int> OnExecuteAsync(CommandLineApplication app, IConsole console)
    {
      var p = @"\\192.168.1.52\media_store_22\siterips\ifm.com\2014\02-February 2014";

      var command = new CommandCheckStatus();
      var result = await command.Execute(this._shellAppContext.Catalog, p);

      TableBuilder tb = new TableBuilder();
      tb.AddRow("ID", "Path", "Status");
      tb.AddRow("--", "----", "------");

      foreach(var ri in result)
        tb.AddRow(ri.CatalogItemId, ri.Path, ri.Status.ToString());

      console.Write(tb.Output());

      return Program.CommandExitResult;
    }
  }
}
