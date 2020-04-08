using System;
using System.Collections.Generic;
using System.Linq;
using Mt.MediaFiles.AppEngine.FileHandlers;

namespace Mt.MediaFiles.AppEngine.Scanning
{
  internal class ScanConfiguration : IScanConfiguration
  {
    private readonly MmConfig _mmConfig;

    internal ScanConfiguration(ScanParameters scanParameters, MmConfig mmConfig)
    {
      this.ScanRootItemName = scanParameters.RootItemName;
      this._mmConfig = mmConfig;
    }

    /// <summary>
    /// IScanConfiguration.
    /// </summary>
    public string ScanRootItemName { get; }

    /// <summary>
    /// IScanConfiguration.
    /// </summary>
    public IReadOnlyList<IFileHandler> FileHandlers { get; set; }

    /// <summary>
    /// IScanConfiguration.
    /// </summary>
    public IReadOnlyList<IScanTask> ScanTasks { get; set; }

    /// <summary>
    /// IScanConfiguration
    /// </summary>
    public bool IsIgnoredEntry(string entryName)
    {
      var result = _mmConfig.Ignore.Any(fn => fn.Equals(entryName, StringComparison.InvariantCultureIgnoreCase));
      return result;
    }
  }
}
