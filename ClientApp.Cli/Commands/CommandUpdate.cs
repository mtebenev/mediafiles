using System;
using System.IO.Abstractions;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.Video.Tasks;
using Mt.MediaFiles.ClientApp.Cli.Core;

namespace Mt.MediaFiles.ClientApp.Cli.Commands
{
  /// <summary>
  /// Walks through the files starting from the current directory and updates the items.
  /// For now used to retrieve video imprints
  /// </summary>
  [Command("update", Description = "Updates information about files starting from the current directory")]
  [ExperimentalCommand]
  internal class CommandUpdate
  {
    public async Task<int> OnExecuteAsync(
      IShellAppContext shellAppContext,
      IFileSystem fileSystem,
      ICatalogTaskUpdateVideoImprintsFactory updateVideoImprintsFactory
      )
    {
      var currentDirectory = fileSystem.Directory.GetCurrentDirectory();
      var catalogItem = await CatalogItemUtils.FindItemByFsPathAsync(
        shellAppContext.Catalog,
        currentDirectory
        );

      if (catalogItem == null)
        throw new InvalidOperationException("Cannot find the current directory in the catalog.");

      var task = updateVideoImprintsFactory.Create(catalogItem);
      await shellAppContext.Catalog.ExecuteTaskAsync(task);

      return Program.CommandExitResult;
    }
  }
}
