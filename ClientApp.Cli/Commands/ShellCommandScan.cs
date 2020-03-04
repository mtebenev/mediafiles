using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaMan.AppEngine.Commands;

namespace Mt.MediaMan.ClientApp.Cli.Commands
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

    /// <summary>
    /// Name for scan root. If not set, then '[SCAN_ROOT]' by default
    /// </summary>
    [Option(LongName = "name", ShortName = "n")]
    public string Name { get; set; }

    protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
    {
      if(string.IsNullOrWhiteSpace(PathAlias))
        throw new InvalidOperationException("Please provide scan path alias");

      var scanPath = PathAlias.Equals("video", StringComparison.InvariantCultureIgnoreCase)
        ? @"C:\_films"
        //? @"\\192.168.1.52\media_store_22"
        : PathAlias.Equals("video", StringComparison.InvariantCultureIgnoreCase) ? @"C:\_books_cat"
        : PathAlias;

      var command = new CommandScanFiles();
      await command.Execute(_executionContext, scanPath, Name);

      return 0;
    }
  }
}
