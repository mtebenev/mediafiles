using System.IO.Abstractions;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaFiles.AppEngine.Matching;
using Mt.MediaFiles.AppEngine.Video.Tasks;
using Mt.MediaFiles.ClientApp.Cli.Ui;

namespace Mt.MediaFiles.ClientApp.Cli.Commands
{
  [Command("search-vdups", Description = "Search for duplicated videos in the catalog.")]
  internal class CommandSearchVideoDuplicates
  {
    public async Task<int> OnExecuteAsync(IShellAppContext shellAppContext, IFileSystem fileSystem, ICatalogTaskSearchVideoDuplicatesFactory taskFactory)
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
      return Program.CommandExitResult;
    }
  }
}
