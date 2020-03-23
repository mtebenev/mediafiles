using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaMan.AppEngine.CatalogStorage;
using Mt.MediaMan.AppEngine.Tasks;

namespace Mt.MediaMan.ClientApp.Cli.Commands
{
  /// <summary>
  /// Scans new files to catalog
  /// </summary>
  [Command("scan", Description = "Scans files to catalog")]
  internal class ShellCommandScan : ShellCommandBase
  {
    [Argument(0, "pathAlias")]
    public string PathAlias { get; set; }

    /// <summary>
    /// Name for scan root. If not set, then '[SCAN_ROOT]' by default
    /// </summary>
    [Option(LongName = "name", ShortName = "n")]
    public string Name { get; set; }

    public async Task<int> OnExecuteAsync(IShellAppContext shellAppContext, ICatalogSettings catalogSettings, ICatalogTaskScanFactory taskFactory)
    {
      if(string.IsNullOrWhiteSpace(PathAlias))
        throw new InvalidOperationException("Please provide scan path alias");

      KeyValuePair<string, string>? mediaRoot = catalogSettings.MediaRoots
        .FirstOrDefault(mr => mr.Key.Equals(PathAlias, StringComparison.InvariantCultureIgnoreCase));
      var scanPath = mediaRoot.HasValue
        ? mediaRoot.Value.Value
        : PathAlias;


      var task = taskFactory.Create(scanPath, this.Name);
      await task.ExecuteAsync(shellAppContext.Catalog);

      return Program.CommandResultContinue;
    }
  }
}
