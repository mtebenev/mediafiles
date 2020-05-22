using System;
using System.Linq;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using Mt.MediaFiles.AppEngine.Tasks;
using Mt.MediaFiles.ClientApp.Cli.Configuration;
using Mt.MediaFiles.ClientApp.Cli.Ui;
using StackExchange.Profiling;

namespace Mt.MediaFiles.ClientApp.Cli.Commands.Shell
{
  /// <summary>
  /// Scans new files to catalog
  /// </summary>
  [Command("scan", Description = "Scan files into the catalog.")]
  internal class CommandShellScan : CommandShellBase
  {
    [Argument(0, "pathAlias")]
    public string PathAlias { get; set; }

    /// <summary>
    /// Name for scan root. If not set, then '[SCAN_ROOT]' by default
    /// </summary>
    [Option(LongName = "name", ShortName = "n")]
    public string Name { get; set; }

    /// <summary>
    /// Scan profile (configuration).
    /// </summary>
    [Option(LongName = "profile", ShortName = "p")]
    public (bool HasValue, ScanProfile ScanProfile) Profile { get; set; }

    public async Task<int> OnExecuteAsync(IShellAppContext shellAppContext, ICatalogSettings catalogSettings, ICatalogTaskScanFactory taskFactory)
    {
      if(string.IsNullOrWhiteSpace(PathAlias))
        throw new InvalidOperationException("Please provide scan path alias");

      var mediaRoot = catalogSettings.MediaRoots
        .FirstOrDefault(mr => mr.Key.Equals(PathAlias, StringComparison.InvariantCultureIgnoreCase));
      var scanPath = mediaRoot.Key != null
        ? mediaRoot.Value
        : PathAlias;

      var scanParameters = ScanParametersBuilder.Create(
        scanPath,
        this.Name,
        this.Profile.HasValue ? this.Profile.ScanProfile : ScanProfile.Default
      );

      var task = taskFactory.Create(scanParameters);
      var profiler = MiniProfiler.StartNew("ShellCommandScan");
      await task.ExecuteAsync(shellAppContext.Catalog);

      await profiler.StopAsync();
      var profileResult = profiler.RenderPlainTextMf();
      shellAppContext.Console.Write(profileResult);

      return Program.CommandResultContinue;
    }
  }
}
