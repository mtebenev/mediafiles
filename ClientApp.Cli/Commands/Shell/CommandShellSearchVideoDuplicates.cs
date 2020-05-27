using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaFiles.AppEngine.Matching;
using Mt.MediaFiles.AppEngine.Video.Tasks;
using Mt.MediaFiles.ClientApp.Cli.Ui;

namespace Mt.MediaFiles.ClientApp.Cli.Commands.Shell
{
  [Command("search-vdups", Description = "Search for duplicated videos in the catalog.")]
  internal class CommandShellSearchVideoDuplicates
  {
    public async Task<int> OnExecuteAsync(IShellAppContext shellAppContext, ICatalogTaskSearchVideoDuplicatesFactory taskFactory)
    {
      var task = taskFactory.Create();
      var matchResult = await shellAppContext.Catalog.ExecuteTaskAsync(task);

      shellAppContext.Console.WriteLine($"{matchResult.MatchGroups.Count} duplicates found:");
      var infoPartAccess = new InfoPartAccessCatalogItem(shellAppContext.Catalog);
      var resultProcessor =
        new MatchResultProcessorVideo(
          infoPartAccess,
          infoPartAccess
        );

      await SearchResultWriter.PrintMatchResult(shellAppContext.Console, resultProcessor, matchResult);

      return Program.CommandResultContinue;
    }
  }
}
