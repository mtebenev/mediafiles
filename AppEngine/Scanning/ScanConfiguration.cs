using System;
using System.Collections.Generic;
using System.Linq;

namespace Mt.MediaFiles.AppEngine.Scanning
{
  /// <summary>
  /// Scan configuration mplementation.
  /// </summary>
  internal class ScanConfiguration : IScanConfiguration
  {
    private readonly MmConfig _mmConfig;
    private readonly List<IScanServiceFactory> _ssFactories;

    internal ScanConfiguration(ScanParameters scanParameters, MmConfig mmConfig, List<IScanServiceFactory> ssFactories)
    {
      this.ScanRootItemName = scanParameters.RootItemName;
      this._mmConfig = mmConfig;
      this._ssFactories = ssFactories;
    }

    /// <summary>
    /// IScanConfiguration.
    /// </summary>
    public string ScanRootItemName { get; }

    /// <summary>
    /// IScanConfiguration.
    /// </summary>
    public IList<IScanService> CreateScanServices()
    {
      var result = this._ssFactories
        .Select(ssf => ssf.Create())
        .ToList();

      return result;
    }

    /// <summary>
    /// IScanConfiguration.
    /// </summary>
    public bool IsIgnoredEntry(string entryName)
    {
      var result = _mmConfig.Ignore.Any(fn => fn.Equals(entryName, StringComparison.InvariantCultureIgnoreCase));
      return result;
    }
  }
}
