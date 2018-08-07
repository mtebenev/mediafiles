using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaMan.AppEngine.Commands;
using Mt.MediaMan.AppEngine.Tools;

namespace Mt.MediaMan.ClientApp.Cli
{
  /// <summary>
  /// Finds duplicate items in catalog
  /// </summary>
  [Command("find-duplicates", Description = "Finds duplicate items in the catalog")]
  internal class ShellCommandFindDuplicates : ShellCommandBase
  {
    private readonly ICommandExecutionContext _executionContext;
    private readonly ShellAppContext _shellAppContext;

    public ShellCommandFindDuplicates(ICommandExecutionContext executionContext, ShellAppContext shellAppContext)
    {
      _executionContext = executionContext;
      _shellAppContext = shellAppContext;
    }

    protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
    {
      var command = new CommandFindDuplicates();
      var result = await command.Execute(_executionContext.Catalog);

      _shellAppContext.Console.WriteLine($"{result.Count} duplicates found:");
      foreach(var duplicates in result)
        await ProcessDuplicates(duplicates);

      return Program.CommandResultContinue;
    }

    private async Task ProcessDuplicates(DuplicateFindResult duplicateResult)
    {
      var console = _shellAppContext.Console;
      var firstItem = await _executionContext.Catalog.GetItemByIdAsync(duplicateResult.CatalogItemIds[0]);

      console.ForegroundColor = ConsoleColor.Yellow;
      console.WriteLine($"{firstItem.Name}");
      console.ResetColor();

      for(int i = 0; i < duplicateResult.FilePaths.Count; i++)
      {
        console.WriteLine($"{i + 1}: {duplicateResult.FilePaths[i]}");
      }
    }
  }
}
