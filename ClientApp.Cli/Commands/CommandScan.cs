using System;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaFiles.AppEngine.Tasks;
using Mt.MediaFiles.ClientApp.Cli.Configuration;
using Mt.MediaFiles.ClientApp.Cli.Ui;
using StackExchange.Profiling;

namespace Mt.MediaFiles.ClientApp.Cli.Commands
{
  [Command("scan", Description = "Scans files in the directory.")]
  internal class CommandScan : AppCommandBase
  {
    [Argument(0, "pathAlias", Description = @"Path to scan, can be one of the following:
- omit to scan the current directory.
- an absolute or relative path to scan.
- media root alias (defined in configuration file).")]
    public string PathAlias { get; set; }

    /// <summary>
    /// Name for the scan root. If not set, then '[SCAN_ROOT]' by default
    /// </summary>
    [Option(LongName = "name", ShortName = "n", Description = "Name in the catalog for the root item.")]
    public string Name { get; set; }

    /// <summary>
    /// Scan profile (configuration).
    /// </summary>
    [Option(LongName = "profile", ShortName = "p", Description = @"Scan configuration. Can be one of the following:
quick: scan only basic file information.
default: scan basic file information + video imprints.
full: scan basic file information + video imprints + thumbnails.")]
    public (bool HasValue, ScanProfile ScanProfile) Profile { get; set; }

    public async Task<int> OnExecuteAsync(
      IConsole console,
      IFileSystem fileSystem,
      ICatalogTaskScanFactory taskFactory
    )
    {
      var catalog = await this.OpenCatalogAsync();
      var catalogSettings = this.GetCatalogSetings();

      string scanPath;
      if(string.IsNullOrEmpty(this.PathAlias))
      {
        scanPath = fileSystem.Directory.GetCurrentDirectory();
      }
      else
      {
        var mediaRoot = catalogSettings.MediaRoots
          .FirstOrDefault(mr => mr.Key.Equals(PathAlias, StringComparison.InvariantCultureIgnoreCase));
        if(mediaRoot.Key != null)
          scanPath = mediaRoot.Value;
        else
        {
          scanPath = fileSystem.Path.IsPathFullyQualified(this.PathAlias)
            ? this.PathAlias
            : fileSystem.Path.GetFullPath(this.PathAlias);
        }
      }

      if(!fileSystem.Directory.Exists(scanPath))
        throw new InvalidOperationException($"Could not find directory or media root: \"{scanPath}\"");

      var scanParameters = ScanParametersBuilder.Create(
        scanPath,
        this.Name,
        this.Profile.HasValue ? this.Profile.ScanProfile : ScanProfile.Default
      );

      var task = taskFactory.Create(scanParameters);
      var profiler = MiniProfiler.StartNew("CommandScan");
      await task.ExecuteAsync(catalog);

      await profiler.StopAsync();
      var profileResult = profiler.RenderPlainTextMf();
      console.Write(profileResult);

      return Program.CommandExitResult;
    }
  }
}
