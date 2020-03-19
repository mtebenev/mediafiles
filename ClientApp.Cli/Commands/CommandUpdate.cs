using System;
using System.IO.Abstractions;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Video.Tasks;

namespace Mt.MediaMan.ClientApp.Cli.Commands
{
  /// <summary>
  /// Walks through the files starting from the current directory and updates the items.
  /// For now used to retrieve video imprints
  /// </summary>
  [Command("update", Description = "Updates information about files starting from the current directory")]
  internal class CommandUpdate
  {
    private readonly IShellAppContext _shellAppContext;
    private readonly IFileSystem _fileSystem;
    private readonly ICatalogTaskUpdateVideoImprintsFactory _updateVideoImprintsFactory;

    public CommandUpdate(IShellAppContext shellAppContext, IFileSystem fileSystem, ICatalogTaskUpdateVideoImprintsFactory updateVideoImprintsFactory)
    {
      this._shellAppContext = shellAppContext;
      this._fileSystem = fileSystem;
      this._updateVideoImprintsFactory = updateVideoImprintsFactory;
    }

    public async Task<int> OnExecuteAsync()
    {
      var currentDirectory = this._fileSystem.Directory.GetCurrentDirectory();
      var catalogItem = await CatalogItemUtils.FindItemByFsPathAsync(
        this._shellAppContext.Catalog,
        currentDirectory);

      if(catalogItem == null)
        throw new InvalidOperationException("Cannot find the current directory in the catalog.");

      var task = this._updateVideoImprintsFactory.Create(catalogItem);
      await this._shellAppContext.Catalog.ExecuteTaskAsync(task);

      return Program.CommandExitResult;
    }
  }
}
