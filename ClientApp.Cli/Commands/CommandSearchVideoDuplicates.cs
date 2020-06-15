using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaFiles.AppEngine.Matching;
using Mt.MediaFiles.AppEngine.Video.Matching;
using Mt.MediaFiles.AppEngine.Video.Tasks;
using Mt.MediaFiles.ClientApp.Cli.Ui;

namespace Mt.MediaFiles.ClientApp.Cli.Commands
{
  [Command("search-vdups", Description = "Search for duplicated videos in the catalog.")]
  internal class CommandSearchVideoDuplicates : AppCommandBase
  {
    public async Task<int> OnExecuteAsync(IConsole console, ICatalogTaskSearchVideoDuplicatesFactory taskFactory)
    {
      var catalog = await this.OpenCatalogAsync();

      var task = taskFactory.Create();
      var matchResult = await catalog.ExecuteTaskAsync(task);

      console.WriteLine($"{matchResult.MatchGroups.Count} duplicates found:");
      var infoPartAccess = new InfoPartAccessCatalogItem(catalog);
      var resultProcessor = 
        new MatchResultProcessorVideo(
          infoPartAccess,
          infoPartAccess
        );

      await SearchResultWriter.PrintMatchResult(console, resultProcessor, matchResult);
      return Program.CommandExitResult;
    }
  }
}
