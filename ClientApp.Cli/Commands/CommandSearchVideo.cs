using System.IO.Abstractions;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using MediaToolkit.Services;
using Mt.MediaFiles.AppEngine.Matching;
using Mt.MediaFiles.AppEngine.Video.Matching;
using Mt.MediaFiles.AppEngine.Video.Tasks;
using Mt.MediaFiles.ClientApp.Cli.Core;
using Mt.MediaFiles.ClientApp.Cli.Ui;
using StackExchange.Profiling;

namespace Mt.MediaFiles.ClientApp.Cli.Commands
{
  [Command("search-video", Description = "Compares videos in local file system with cataloged videos.")]
  internal class CommandSearchVideo : AppCommandBase
  {
    [Argument(0, "path", Description = @"The path to scan, can be one of the following:
- omit to search for video files in the current directory
- an absolute or relative path to a directory with videos to search for.")]
    public string ThePath { get; set; }

    public async Task<int> OnExecuteAsync(
      IFileSystem fileSystem,
      IConsole console,
      IMediaToolkitService mediaToolkitService,
      IPathArgumentResolver pathResolver,
      ICatalogTaskSearchVideoFactory taskFactory
    )
    {
      var catalog = await this.OpenCatalogAsync();
      var catalogSettings = this.GetCatalogSetings();

      var paths = pathResolver.ResolveMany(this.ThePath, catalogSettings);
      var task = taskFactory.Create(paths);
      var profiler = MiniProfiler.StartNew("CommandSearchVideo");
      var matchResult = await catalog.ExecuteTaskAsync(task);

      console.WriteLine($"{matchResult.MatchGroups.Count} duplicates found:");
      await profiler.StopAsync();

      var infoPartAccessCatalog = new InfoPartAccessCatalogItem(catalog);
      var infoPartAccessFs = new InfoPartAccessLocalFile(fileSystem, mediaToolkitService, paths);

      var resultProcessor =
        new MatchResultProcessorVideo(
          infoPartAccessFs,
          infoPartAccessCatalog
        );

      await SearchResultWriter.PrintMatchResult(console, resultProcessor, matchResult);

      var profileResult = profiler.RenderPlainTextMf();
      console.Write(profileResult);

      return Program.CommandExitResult;
    }
  }
}
