using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaMan.AppEngine.Commands;
using Mt.MediaMan.AppEngine.Common;
using Mt.MediaMan.AppEngine.Tools;

namespace Mt.MediaMan.ClientApp.Cli.Commands
{
  /// <summary>
  /// Finds duplicate items in catalog
  /// </summary>
  [Command("find-duplicates", Description = "Finds duplicate items in the catalog")]
  internal class ShellCommandFindDuplicates : ShellCommandBase
  {
    private readonly IShellAppContext _shellAppContext;

    public ShellCommandFindDuplicates(IShellAppContext shellAppContext)
    {
      this._shellAppContext = shellAppContext;
    }

    /// <summary>
    /// ShellCommandBase.
    /// </summary>
    public async Task<int> OnExecuteAsync()
    {
      var command = new CommandFindDuplicates();
      var result = await command.Execute(this._shellAppContext.Catalog);

      long totalWastedSize = 0;
      this._shellAppContext.Console.WriteLine($"{result.Count} duplicates found:");
      foreach(var duplicates in result)
      {
        var wastedSize = await ProcessDuplicates(duplicates);
        totalWastedSize += wastedSize;
      }

      this._shellAppContext.Console.WriteLine($"Total wasted size: {StringUtils.BytesToString(totalWastedSize)}");

      return Program.CommandResultContinue;
    }

    /// <summary>
    /// Prints duplicate info and returns wasted file size in bytes
    /// </summary>
    private async Task<long> ProcessDuplicates(DuplicateFindResult duplicateResult)
    {
      var console = _shellAppContext.Console;
      var firstItem = await this._shellAppContext.Catalog.GetItemByIdAsync(duplicateResult.FileInfos[0].CatalogItemId);

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
