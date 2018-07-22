using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaMan.AppEngine.Commands;

namespace Mt.MediaMan.ClientApp.Cli
{
  /// <summary>
  /// Scans new files to catalog
  /// </summary>
  [Command("scan", Description = "Scans files to catalog")]
  internal class ShellCommandScan : ShellCommandBase
  {
    private readonly ICommandExecutionContext _executionContext;

    public ShellCommandScan(ICommandExecutionContext executionContext)
    {
      _executionContext = executionContext;
    }

    [Argument(0, "pathAlias")]
    public string PathAlias { get; set; }

    protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
    {
      if(String.IsNullOrWhiteSpace(PathAlias))
        throw new InvalidOperationException("Please provide scan path alias");

      var scanPath = PathAlias.Equals("video", StringComparison.InvariantCultureIgnoreCase)
        ? @"C:\_films"
        : PathAlias.Equals("video", StringComparison.InvariantCultureIgnoreCase) ? @"C:\_books_cat"
        : PathAlias;

      var command = new CommandScanFiles();
      await command.Execute(_executionContext, scanPath);

      return 0;
    }
  }
}
