using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Mt.MediaMan.AppEngine.Tasks;

namespace Mt.MediaMan.ClientApp.Cli.Commands
{
  /// <summary>
  /// Command finds the video duplicates.
  /// </summary>
  [Command("find-vdups", Description = "Finds duplicate videos")]
  internal class ShellCommandFindVideoDuplicates : ShellCommandBase
  {
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Ctor.
    /// </summary>
    public ShellCommandFindVideoDuplicates(IServiceProvider serviceProvider)
    {
      this._serviceProvider = serviceProvider;
    }

    /// <summary>
    /// ShellCommandBase.
    /// </summary>
    protected override async Task<int> OnExecuteAsync()
    {
      var task = ActivatorUtilities.CreateInstance<CatalogTaskFindVideoDuplicates>(this._serviceProvider);
      await task.ExecuteAsync(null);

      return Program.CommandResultContinue;
    }
  }
}
