using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaFiles.AppEngine.Tools;
using Mt.MediaFiles.AppEngine.Video.Tasks;

namespace Mt.MediaFiles.ClientApp.Cli.Commands
{
  [Command("search-vdups", Description = "Search for duplicated videos in the catalog.")]
  internal class ShellCommandSearchVideoDuplicates
  {
    public async Task<int> OnExecuteAsync(IShellAppContext shellAppContext, ICatalogTaskSearchVideoDuplicatesFactory taskFactory)
    {
      var task = taskFactory.Create();
      var result = await shellAppContext.Catalog.ExecuteTaskAsync(task);

      shellAppContext.Console.WriteLine($"{result.Count} duplicates found:");
      foreach(var duplicates in result)
      {
        await this.ProcessDuplicates(shellAppContext, duplicates);
      }

      return Program.CommandResultContinue;
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
