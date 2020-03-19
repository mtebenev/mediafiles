using System;
using System.IO.Abstractions;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Commands;

namespace Mt.MediaMan.ClientApp.Cli.Commands
{
  /// <summary>
  /// Checks the status of the files in the current directory.
  /// </summary>
  [Command("status", Description = "Checks the status of the files in the current directory")]
  internal class CommandCheckStatus
  {
    public async Task<int> OnExecuteAsync(IShellAppContext shellAppContext, IFileSystem fileSystem, ICatalogTaskCheckStatusFactory taskFactory, IConsole console)
    {
      var currentDirectory = fileSystem.Directory.GetCurrentDirectory();
      var catalogItem = await CatalogItemUtils.FindItemByFsPathAsync(
        shellAppContext.Catalog,
        currentDirectory);

      if(catalogItem == null)
        throw new InvalidOperationException("Cannot find the current directory in the catalog.");

      var task = taskFactory.Create(catalogItem);
      var result = await shellAppContext.Catalog.ExecuteTaskAsync(task);

      TableBuilder tb = new TableBuilder();
      tb.AddRow("ID", "Path", "Status");
      tb.AddRow("--", "----", "------");

      foreach(var ri in result)
        tb.AddRow(ri.CatalogItemId, ri.Path, ri.Status.ToString());

      console.Write(tb.Output());

      return Program.CommandExitResult;
    }
  }
}
