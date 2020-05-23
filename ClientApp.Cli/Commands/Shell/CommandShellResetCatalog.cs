using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace Mt.MediaFiles.ClientApp.Cli.Commands.Shell
{
  /// <summary>
  /// Deletes all the data in the existing catalog.
  /// </summary>
  [Command("reset-catalog", Description = "Reset the catalog storage.")]
  internal class CommandShellResetCatalog : CommandShellBase
  {
    public async Task<int> OnExecuteAsync(ShellAppContext shellAppContext, IServiceProvider serviceProvider)
    {
      await shellAppContext.ResetCatalogStorage(serviceProvider);
      shellAppContext.Console.WriteLine("All data cleared.");
      await shellAppContext.OpenCatalog(serviceProvider);
      return Program.CommandResultContinue;
    }
  }
}
