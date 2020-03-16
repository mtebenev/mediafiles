using System;
using System.Threading.Tasks;
using AppEngine.Video.VideoImprint;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Mt.MediaMan.AppEngine.Commands;

namespace Mt.MediaMan.ClientApp.Cli.Commands
{
  /// <summary>
  /// Walks through the files starting from the current directory and updates the items.
  /// For now used to retrieve video imprints
  /// </summary>
  [Command("update", Description = "Updates information about files starting from the current directory")]
  internal class Update
  {
    private readonly ICommandExecutionContext _executionContext;
    private readonly IServiceProvider _serviceProvider;

    public Update(ICommandExecutionContext executionContext, IServiceProvider serviceProvider)
    {
      this._executionContext = executionContext;
      this._serviceProvider = serviceProvider;
    }

    public async Task<int> OnExecuteAsync(CommandLineApplication app, IConsole console)
    {
      //var p = @"\\192.168.1.52\media_store_22\siterips";
      var p = @"C:\_films";

      var command = ActivatorUtilities.CreateInstance<CommandUpdate>(this._serviceProvider);
      await command.ExecuteAsync(this._executionContext, p);

      return Program.CommandExitResult;
    }
  }
}
