using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaMan.AppEngine.Commands;

namespace Mt.MediaMan.ClientApp.Cli.Commands
{
  /// <summary>
  /// Checks the status of the files in the current directory.
  /// </summary>
  [Command("status", Description = "Checks the status of the files in the current directory")]
  internal class Status
  {
    private readonly ICommandExecutionContext _executionContext;
    private readonly ShellAppContext _shellAppContext;

    public Status(ICommandExecutionContext executionContext, ShellAppContext shellAppContext)
    {
      _executionContext = executionContext;
      _shellAppContext = shellAppContext;
    }

    public Task<int> OnExecuteAsync(CommandLineApplication app, IConsole console)
    {
      console.WriteLine("Running status command...");
      return Task.FromResult(Program.CommandExitResult);
    }
  }
}
