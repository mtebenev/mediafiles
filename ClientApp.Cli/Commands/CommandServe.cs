using McMaster.Extensions.CommandLineUtils;
using Mt.MediaFiles.ClientApp.Cli.Core;
using Mt.MediaFiles.FeatureLib.Api.Tasks;
using System;
using System.Threading.Tasks;

namespace Mt.MediaFiles.ClientApp.Cli.Commands
{
  /// <summary>
  /// Starts the API server.
  /// </summary>
  [Command("serve", Description = "Starts the API server.")]
  [ExperimentalCommand]
  internal class CommandServe
  {
    public async Task<int> OnExecuteAsync(IShellAppContext shellAppContext, IServiceProvider serviceProvider)
    {
      var task = new CatalogTaskServe(serviceProvider);
      await shellAppContext.Catalog.ExecuteTaskAsync(task);
      return Program.CommandExitResult;
    }
  }
}
