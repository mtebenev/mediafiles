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
    /// Creates scan parameters by scan profile.
    /// </summary>
    public static ScanParameters Create(string scanPath, string rootItemName, ScanProfile scanProfile)
    {
      (List<string> scanTaskIds, List<string> fileHandlerIds) objectIds = (null, null);

      switch(scanProfile)
      {
        case ScanProfile.Quick:
          objectIds = GetObjectIdsQuick();
          break;
        case ScanProfile.Full:
          objectIds = GetObjectIdsFull();
          break;
        default:
          objectIds = GetObjectIdsDefault();
          break;
      }

      var result = new ScanParameters(scanPath, rootItemName, objectIds.scanTaskIds, objectIds.fileHandlerIds);
      return result;
    }

    private static (List<string> scanTaskIds, List<string> fileHandlerIds) GetObjectIdsQuick()
    {
      var scanTaskIds = new List<string>();
      var fileHandlerIds = new List<string>();

      return (scanTaskIds, fileHandlerIds);
    }

    private static (List<string> scanTaskIds, List<string> fileHandlerIds) GetObjectIdsFull()
    {
      var scanTaskIds = new List<string>
      {
        AppEngine.HandlerIds.ScanTaskScanInfo,
        AppEngine.Video.HandlerIds.ScanTaskVideoImprints
      };

      var fileHandlerIds = new List<string>
      {
        AppEngine.HandlerIds.FileHandlerVideo
      };

      return (scanTaskIds, fileHandlerIds);
    }

    private static (List<string> scanTaskIds, List<string> fileHandlerIds) GetObjectIdsDefault()
    {
      var scanTaskIds = new List<string>
      {
        AppEngine.Video.HandlerIds.ScanTaskVideoImprints
      };

      var fileHandlerIds = new List<string>
      {
        AppEngine.HandlerIds.FileHandlerVideo
      };

      return (scanTaskIds, fileHandlerIds);
    }
  }
}
