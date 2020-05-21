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

    /// <summary>
    /// The quick scan profile.
    /// </summary>
    private static (List<string> scanTaskIds, List<string> fileHandlerIds) GetObjectIdsQuick()
    {
      var scanTaskIds = new List<string>();
      var fileHandlerIds = new List<string>();

      return (scanTaskIds, fileHandlerIds);
    }

    /// <summary>
    /// The default scan profile.
    /// </summary>
    private static (List<string> scanTaskIds, List<string> fileHandlerIds) GetObjectIdsDefault()
    {
      var scanTaskIds = new List<string>
      {
        AppEngine.HandlerIds.ScanSvcScanInfo,
        AppEngine.Video.HandlerIds.ScanSvcVideoImprints
      };

      var fileHandlerIds = new List<string>
      {
        AppEngine.HandlerIds.FileHandlerVideo
      };

      return (scanTaskIds, fileHandlerIds);
    }

    /// <summary>
    /// The full scan profile.
    /// </summary>
    private static (List<string> scanTaskIds, List<string> fileHandlerIds) GetObjectIdsFull()
    {
      var scanTaskIds = new List<string>
      {
        AppEngine.HandlerIds.ScanSvcScanInfo,
        AppEngine.Video.HandlerIds.ScanSvcVideoImprints,
        AppEngine.Video.HandlerIds.ScanSvcThumbnail
      };

      var fileHandlerIds = new List<string>
      {
        AppEngine.HandlerIds.FileHandlerVideo
      };

      return (scanTaskIds, fileHandlerIds);
    }
  }
}
