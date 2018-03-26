using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaMan.AppEngine.Commands;

namespace Mt.MediaMan.ClientApp.Cli
{
  /// <summary>
  /// Scans new files to catalog
  /// </summary>
  [Command(Description = "Scans files to catalog")]
  internal class ShellCommandScan : ShellCommandBase
  {
    private readonly ICommandExecutionContext _executionContext;

    public ShellCommandScan(ICommandExecutionContext executionContext)
    {
      _executionContext = executionContext;
    }

    protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
    {
      var scanPath = @"C:\_films";
      var command = new CommandScanFiles();
      await command.Execute(_executionContext.Catalog, scanPath);

      return 0;
    }
  }
}
