using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaFiles.AppEngine.Tools;
using Mt.MediaFiles.AppEngine.Video.Tasks;
using Mt.MediaFiles.ClientApp.Cli.Core;
using Mt.MediaFiles.ClientApp.Cli.Ui;
using StackExchange.Profiling;

namespace Mt.MediaFiles.ClientApp.Cli.Commands
{
  [Command("search-video", Description = "Compares videos in local file system with cataloged videos.")]
  internal class CommandSearchVideo
  {
    [Argument(0, "path", Description = @"The path to scan, can be one of the following:
- omit to search for video files in the current directory
- an absolute or relative path to a directory with videos to search for.")]
    public string ThePath { get; set; }

    public async Task<int> OnExecuteAsync(
      IShellAppContext shellAppContext,
      IPathArgumentResolver pathResolver,
      ICatalogTaskSearchVideoFactory taskFactory
    )
    {
      var paths = pathResolver.ResolveMany(this.ThePath);
      var task = taskFactory.Create(paths);
      var profiler = MiniProfiler.StartNew("CommandSearchVideo");
      var result = await shellAppContext.Catalog.ExecuteTaskAsync(task);

      shellAppContext.Console.WriteLine($"{result.Count} duplicates found:");
      foreach(var duplicates in result)
      {
        await this.ProcessDuplicates(shellAppContext, duplicates);
      }

      await profiler.StopAsync();
      var profileResult = profiler.RenderPlainTextMf();
      shellAppContext.Console.Write(profileResult);

      return Program.CommandExitResult;
    }

    /// <summary>
    /// Prints duplicate info
    /// </summary>
    private async Task<long> ProcessDuplicates(IShellAppContext shellAppContext, DuplicateFindResult duplicateResult)
    {
      var console = shellAppContext.Console;
      var firstItem = await shellAppContext.Catalog.GetItemByIdAsync(duplicateResult.FileInfos[0].CatalogItemId);

      long wastedSize = 0; // Total wasted size in bytes

      console.ForegroundColor = ConsoleColor.Yellow;
      console.WriteLine($"{firstItem.Path}");
      console.ResetColor();

      for(var i = 0; i < duplicateResult.FileInfos.Count; i++)
      {
        if(i > 0)
          wastedSize += duplicateResult.FileInfos[i].FileSize;

        console.WriteLine($"{i + 1}: {duplicateResult.FileInfos[i].FilePath}");
      }

      return wastedSize;
    }
  }
}
