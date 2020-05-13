using McMaster.Extensions.CommandLineUtils;
using System;
using System.Threading.Tasks;

namespace Mt.MediaFiles.ClientApp.Cli.Commands
{
  [Command("reset-catalog", Description = "Reset the catalog storage.")]
  internal class CommandResetCatalog
  {
    public async Task<int> OnExecuteAsync(ShellAppContext shellAppContext, IServiceProvider serviceProvider)
    {
      await shellAppContext.ResetCatalogStorage(serviceProvider);
      shellAppContext.Console.WriteLine("All data cleared.");
      return Program.CommandExitResult;
    }
  }
}
