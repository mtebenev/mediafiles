using System.Collections.Generic;
using Mt.MediaMan.AppEngine.FileHandlers;

namespace Mt.MediaMan.AppEngine.Scanning
{
  /// <summary>
  /// The scan configuration interface.
  /// </summary>
  internal interface IScanConfiguration
  {
    /// <summary>
    /// The subtasks to perform.
    /// </summary>
    IReadOnlyList<ISubTask> SubTasks { get; }

    /// <summary>
    /// Configured file handlers.
    /// </summary>
    IReadOnlyList<IFileHandler> FileHandlers { get; }

    /// <summary>
    /// Given scan root item name.
    /// </summary>
    string ScanRootItemName { get; }

    /// <summary>
    /// Checks if an entry with given name should be ignored during scanning.
    /// </summary>
    bool IsIgnoredEntry(string entryName);
  }
}
