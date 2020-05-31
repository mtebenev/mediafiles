using System.Collections.Generic;

namespace Mt.MediaFiles.AppEngine.Scanning
{
  /// <summary>
  /// The scan configuration interface.
  /// </summary>
  internal interface IScanConfiguration
  {
    /// <summary>
    /// Given scan root item name.
    /// </summary>
    string ScanRootItemName { get; }

    /// <summary>
    /// Checks if an entry with given name should be ignored during scanning.
    /// </summary>
    bool IsIgnoredEntry(string entryName);

    /// <summary>
    /// Creates set of scan services to use.
    /// </summary>
    IList<IScanService> CreateScanServices();
  }
}
