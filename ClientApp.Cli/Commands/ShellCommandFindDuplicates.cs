using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaFiles.AppEngine.Commands;
using Mt.MediaMan.AppEngine.Common;
using Mt.MediaMan.AppEngine.Tools;

namespace Mt.MediaFiles.ClientApp.Cli.Commands
{
  /// <summary>
  /// Finds duplicate items in catalog
  /// </summary>
  [Command("find-duplicates", Description = "Finds duplicate items in the catalog")]
  internal class ShellCommandFindDuplicates : ShellCommandBase
  {
    /// <summary>
    /// ShellCommandBase.
    /// </summary>
    public async Task<int> OnExecuteAsync(IShellAppContext shellAppContext)
    {
      var command = new CommandFindDuplicates();
      var result = await command.Execute(shellAppContext.Catalog);

      long totalWastedSize = 0;
      shellAppContext.Console.WriteLine($"{result.Count} duplicates found:");
      foreach(var duplicates in result)
      {
        var wastedSize = await this.ProcessDuplicates(shellAppContext, duplicates);
        totalWastedSize += wastedSize;
      }

      shellAppContext.Console.WriteLine($"Total wasted size: {StringUtils.BytesToString(totalWastedSize)}");

      return Program.CommandResultContinue;
    }

    /// <summary>
    /// Prints duplicate info and returns wasted file size in bytes
    /// </summary>
    private async Task<long> ProcessDuplicates(IShellAppContext shellAppContext, DuplicateFindResult duplicateResult)
    {
      var console = shellAppContext.Console;
      var firstItem = await shellAppContext.Catalog.GetItemByIdAsync(duplicateResult.FileInfos[0].CatalogItemId);

      long wastedSize = 0; // Total wasted size in bytes

      console.ForegroundColor = ConsoleColor.Yellow;
      console.WriteLine($"{firstItem.Path}");
      console.ResetColor();

      for(var i = 0; i < duplicateResult.FileInfos.Count; i++)
      {
        if(i > 0)
          wastedSize += duplicateResult.FileInfos[i].FileSize;

        console.WriteLine($"{i + 1}: {duplicateResult.FileInfos[i].FilePath}");
      }

      return wastedSize;
    }
  }
}
