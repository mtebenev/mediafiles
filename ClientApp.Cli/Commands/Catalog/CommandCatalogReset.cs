using McMaster.Extensions.CommandLineUtils;
using Mt.MediaFiles.AppEngine.Tasks;
using System.Threading.Tasks;

namespace Mt.MediaFiles.ClientApp.Cli.Commands.Catalog
{
  [Command("reset", Description = "Resets the catalog storage.")]
  internal class CommandCatalogReset : AppCommandBase
  {
    public async Task<int> OnExecuteAsync(IConsole console)
    {
      var catalog = await this.OpenCatalogAsync();
      var catalogSettings = this.GetCatalogSetings();
      var task = new CatalogTaskResetStorage(catalogSettings.CatalogName, catalogSettings.ConnectionString);
      await task.ExecuteAsync(catalog);
      console.WriteLine("All data cleared.");

      return Program.CommandExitResult;
    }
  }
}
