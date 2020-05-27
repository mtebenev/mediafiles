using McMaster.Extensions.CommandLineUtils;
using Mt.MediaFiles.AppEngine.Matching;
using System;
using System.Linq;
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
    public static async Task PrintMatchResult(IConsole console, MatchResultProcessorVideo resultProcessor, MatchResult matchResult)
    {
      await foreach(var mg in resultProcessor.ProcessAsync(matchResult))
      {
        PrintGroups(console, mg);
      }
    }

    private static void PrintGroups(IConsole console, MatchOutputGroup matchOutputGroup)
    {
      console.ForegroundColor = ConsoleColor.Yellow;
      console.WriteLine(matchOutputGroup.BaseItem);
      console.ResetColor();

      for(int i = 0; i < matchOutputGroup.Items.Count; i++)
      {
        console.WriteLine($"> {matchOutputGroup.Items[i].Item}");
        PrintDifferences(console, matchOutputGroup.Items[i]);
        console.WriteLine();
      }
    }

    private static void PrintDifferences(IConsole console, MatchOutputItem matchedItem)
    {
      var diffProps = matchedItem
        .Properties
        .Where(p => p.Qualification != ComparisonQualification.Equal)
        .ToList();
      if(diffProps.Count > 0)
      {
        for(int i = 0; i < diffProps.Count; i++)
        {
          console.Write(i > 0 ? ", " : "  [");
          console.Write($"{diffProps[i].Name}: ");
          console.ForegroundColor = GetQualificationColor(diffProps[i].Qualification);
          console.Write(diffProps[i].Value);
          if(!string.IsNullOrEmpty(diffProps[i].RelativeValue))
          {
            console.Write($" ({diffProps[i].RelativeValue})");
          }
          console.ResetColor();
        }
        console.WriteLine("]");
      }
    }

    private static ConsoleColor GetQualificationColor(ComparisonQualification qualification)
    {
      var result = qualification == ComparisonQualification.Better
                  ? ConsoleColor.Green
                  : qualification == ComparisonQualification.Worse
                    ? ConsoleColor.Red
                    : ConsoleColor.Yellow;

      return result;
    }
  }
}
