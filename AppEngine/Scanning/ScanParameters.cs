using System.Collections.Generic;

namespace Mt.MediaFiles.AppEngine.Scanning
{
  /// <summary>
  /// 'User-friendly' public scan parameters (inputs for scanner).
  /// </summary>
  public class ScanParameters
  {
    public ScanParameters(string scanPath, string rootItemName, List<string> scanSvcIds, List<string> fileHandlerIds)
    {
      this.ScanSvcIds = scanSvcIds;
      this.FileHandlerIds = fileHandlerIds;
      this.ScanPath = scanPath;
      this.RootItemName = rootItemName;
    }

    /// <summary>
    /// Given name for the scan root item.
    /// </summary>
    public string RootItemName { get; private set; }

    /// <summary>
    /// FS path for scanning.
    /// </summary>
    public string ScanPath { get; private set; }

    /// <summary>
    /// Ids of the scan services to use.
    /// </summary>
    public IReadOnlyList<string> ScanSvcIds { get; private set; }

    /// <summary>
    /// File handlers to be used.
    /// </summary>
    public IReadOnlyList<string> FileHandlerIds { get; private set; }
  }
}
