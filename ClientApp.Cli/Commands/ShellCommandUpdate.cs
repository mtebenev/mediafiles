using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaFiles.AppEngine.Video.Tasks;

namespace Mt.MediaFiles.ClientApp.Cli.Commands
{
  [Command("update", Description = "Update items starting from the current location or item id.")]
  internal class ShellCommandUpdate : ShellCommandBase
  {
    private readonly IShellAppContext _shellAppContext;
    private readonly ICatalogTaskUpdateVideoImprintsFactory _updateVideoImprintsFactory;

    /// <summary>
    /// Ctor.
    /// </summary>
    public ShellCommandUpdate(IShellAppContext shellAppContext, ICatalogTaskUpdateVideoImprintsFactory updateVideoImprintsFactory)
    {
      this._shellAppContext = shellAppContext;
      this._updateVideoImprintsFactory = updateVideoImprintsFactory;
    }

    /// <summary>
    /// ShellCommandBase.
    /// </summary>
    public async Task<int> OnExecuteAsync()
    {
      var currentItem = _shellAppContext.CurrentItem;
      var task = this._updateVideoImprintsFactory.Create(currentItem);
      await this._shellAppContext.Catalog.ExecuteTaskAsync(task);

      return Program.CommandResultContinue;
    }
  }
}
