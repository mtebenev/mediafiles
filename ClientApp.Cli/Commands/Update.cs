using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaMan.AppEngine.Commands;

namespace Mt.MediaMan.ClientApp.Cli.Commands
{
  /// <summary>
  /// Walks through the files starting from the current directory and updates the items.
  /// For now used to retrieve video imprints
  /// </summary>
  [Command("update", Description = "Updates information about files starting from the current directory")]
  internal class Update
  {
    private readonly ICommandExecutionContext _executionContext;
    private readonly ShellAppContext _shellAppContext;

    public Update(ICommandExecutionContext executionContext, ShellAppContext shellAppContext)
    {
      _executionContext = executionContext;
      _shellAppContext = shellAppContext;
    }

    public async Task<int> OnExecuteAsync(CommandLineApplication app, IConsole console)
    {
      var p = @"\\192.168.1.52\media_store_22\siterips";

      var command = new CommandUpdate();
      await command.ExecuteAsync(this._executionContext, p);

      return Program.CommandExitResult;
    }
  }
}
