using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace Mt.MediaFiles.ClientApp.Cli.Commands
{
  /// <summary>
  /// Deletes all the data in the existing catalog.
  /// </summary>
  [Command("reset-catalog", Description = "Resets catalog storage")]
  internal class ShellCommandResetCatalog : ShellCommandBase
  {
    public async Task<int> OnExecuteAsync(ShellAppContext shellAppContext, IServiceProvider serviceProvider)
    {
      await shellAppContext.ResetCatalogStorage(serviceProvider);
      return Program.CommandResultContinue;
    }
  }
}
