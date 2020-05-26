using McMaster.Extensions.CommandLineUtils;
using Mt.MediaFiles.AppEngine.Matching;
using System;
using System.Threading.Tasks;

namespace Mt.MediaFiles.ClientApp.Cli.Ui
{
  /// <summary>
  /// Utility class for writing search results to console.
  /// </summary>
  internal static class SearchResultWriter
  {
    /// <summary>
    /// Performs result processing and prints the output.
    /// </summary>
    public static async Task PrintMatchResult(MatchResultProcessorVideo resultProcessor, MatchResult matchResult, IConsole console)
    {
      await foreach(var mg in resultProcessor.ProcessAsync(matchResult))
      {
        ProcessDuplicates(mg, console);
      }
    }

    private static void ProcessDuplicates(MatchOutputGroup matchOutputGroup, IConsole console)
    {
      console.ForegroundColor = ConsoleColor.Yellow;
      console.WriteLine(matchOutputGroup.BaseItem);
      console.ResetColor();

      for(int i = 0; i < matchOutputGroup.Items.Count; i++)
      {
        console.WriteLine($"> {matchOutputGroup.Items[i].Item}");
      }
    }
  }
}
