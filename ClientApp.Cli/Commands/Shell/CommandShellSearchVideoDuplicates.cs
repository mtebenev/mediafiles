using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaFiles.AppEngine.Matching;
using Mt.MediaFiles.AppEngine.Video.Matching;
using Mt.MediaFiles.AppEngine.Video.Tasks;
using Mt.MediaFiles.ClientApp.Cli.Ui;

namespace Mt.MediaFiles.ClientApp.Cli.Commands.Shell
{
  [Command("search-vdups", Description = "Search for duplicated videos in the catalog.")]
  internal class CommandShellSearchVideoDuplicates : CommandShellBase
  {
    public async Task<int> OnExecuteAsync(ICatalogTaskSearchVideoDuplicatesFactory taskFactory)
    {
      var task = taskFactory.Create();
      var matchResult = await this.ShellAppContext.Catalog.ExecuteTaskAsync(task);

      this.ShellAppContext.Console.WriteLine($"{matchResult.MatchGroups.Count} duplicates found:");
      var infoPartAccess = new InfoPartAccessCatalogItem(this.ShellAppContext.Catalog);
      var resultProcessor =
        new MatchResultProcessorVideo(
          infoPartAccess,
          infoPartAccess
        );

      await SearchResultWriter.PrintMatchResult(this.ShellAppContext.Console, resultProcessor, matchResult);

      return Program.CommandResultContinue;
    }
  }
}
