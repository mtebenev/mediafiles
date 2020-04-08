using System.Collections.Generic;
using Mt.MediaFiles.AppEngine.Scanning;

namespace Mt.MediaFiles.ClientApp.Cli.Configuration
{
  /// <summary>
  /// Builds scan parameters for well-known scan configurations.
  /// </summary>
  internal static class ScanParametersBuilder
  {
    /// <summary>
    /// Factory method: normal scan (video imprints sub-task).
    /// </summary>
    public static ScanParameters Create(string scanPath, string rootItemName)
    {
      var subTaskIds = new List<string>
      {
        AppEngine.Video.HandlerIds.ScanTaskVideoImprints
      };

      var fileHandlerIds = new List<string>
      {
        AppEngine.HandlerIds.FileHandlerVideo
      };

      var result = new ScanParameters(scanPath, rootItemName, subTaskIds, fileHandlerIds);
      return result;
    }

    /// <summary>
    /// Factory method: quick scan (no sub-tasks, all file handlers).
    /// </summary>
    public static ScanParameters CreateQuick(string scanPath, string rootItemName)
    {
      var subTaskIds = new List<string>();
      var fileHandlerIds = new List<string>();

      var result = new ScanParameters(scanPath, rootItemName, subTaskIds, fileHandlerIds);
      return result;
    }

    /// <summary>
    /// Factory method: full scan.
    /// Sub-tasks: video imprints, metadata scanning
    /// All file handlers.
    /// </summary>
    public static ScanParameters CreateFull(string scanPath, string rootItemName)
    {
      var subTaskIds = new List<string>
      {
        AppEngine.HandlerIds.ScanTaskScanInfo,
        AppEngine.Video.HandlerIds.ScanTaskVideoImprints
      };

      var fileHandlerIds = new List<string>
      {
        AppEngine.HandlerIds.FileHandlerVideo
      };

      var result = new ScanParameters(scanPath, rootItemName, subTaskIds, fileHandlerIds);
      return result;
    }
  }
}
