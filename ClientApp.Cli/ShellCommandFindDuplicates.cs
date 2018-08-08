using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaMan.AppEngine.Commands;
using Mt.MediaMan.AppEngine.Common;
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

      long totalWastedSize = 0;
      _shellAppContext.Console.WriteLine($"{result.Count} duplicates found:");
      foreach(var duplicates in result)
      {
        var wastedSize = await ProcessDuplicates(duplicates);
        totalWastedSize += wastedSize;
      }

      _shellAppContext.Console.WriteLine($"Total wasted size: {StringUtils.BytesToString(totalWastedSize)}");

      return Program.CommandResultContinue;
    }

    /// <summary>
    /// Prints duplicate info and returns wasted file size in bytes
    /// </summary>
    private async Task<long> ProcessDuplicates(DuplicateFindResult duplicateResult)
    {
      var console = _shellAppContext.Console;
      var firstItem = await _executionContext.Catalog.GetItemByIdAsync(duplicateResult.FileInfos[0].CatalogItemId);

      long wastedSize = 0; // Total wasted size in bytes

      console.ForegroundColor = ConsoleColor.Yellow;
      console.WriteLine($"{firstItem.Name}");
      console.ResetColor();

      for(int i = 0; i < duplicateResult.FileInfos.Count; i++)
      {
        if(i > 0)
          wastedSize += duplicateResult.FileInfos[i].FileSize;

        console.WriteLine($"{i + 1}: {duplicateResult.FileInfos[i].FilePath}");
      }

      return wastedSize;
    }
  }
}
