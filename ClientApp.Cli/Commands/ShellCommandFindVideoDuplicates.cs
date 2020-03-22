using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaMan.AppEngine.Tasks;

namespace Mt.MediaMan.ClientApp.Cli.Commands
{
  /// <summary>
  /// Command finds the video duplicates.
  /// </summary>
  [Command("find-vdups", Description = "Finds duplicate videos")]
  internal class ShellCommandFindVideoDuplicates : ShellCommandBase
  {
    /// <summary>
    /// ShellCommandBase.
    /// </summary>
    public async Task<int> OnExecuteAsync(IShellAppContext shellAppContext, ICatalogTaskFindVideoDuplicatesFactory taskFactory)
    {
      var task = taskFactory.Create();
      await shellAppContext.Catalog.ExecuteTaskAsync(task);

      return Program.CommandResultContinue;
    }
  }
}
