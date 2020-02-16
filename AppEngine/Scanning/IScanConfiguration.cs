using System.Collections.Generic;
using Mt.MediaMan.AppEngine.FileHandlers;

namespace Mt.MediaMan.AppEngine.Scanning
{
  /// <summary>
  /// The scan configuration interface.
  /// </summary>
  internal interface IScanConfiguration
  {
    IReadOnlyList<IFileHandler> FileHandlers { get; }
    string ScanRootItemName { get; }

    /// <summary>
    /// Checks if an entry with given name should be ignored during scanning.
    /// </summary>
    bool IsIgnoredEntry(string entryName);
  }
}
