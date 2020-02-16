using System;
using System.Collections.Generic;
using System.Linq;
using Mt.MediaMan.AppEngine.FileHandlers;

namespace Mt.MediaMan.AppEngine.Scanning
{
  internal class ScanConfiguration : IScanConfiguration
  {
    private readonly List<IFileHandler> _fileHandlers;
    private readonly MmConfig _mmConfig;

    public ScanConfiguration(string scanRootItemName, MmConfig mmConfig)
    {
      ScanRootItemName = scanRootItemName;
      _fileHandlers = new List<IFileHandler>
      {
        new FileHandlerVideo(),
        new FileHandlerEpub()
      };
      _mmConfig = mmConfig;
    }

    public string ScanRootItemName { get; }
    public IReadOnlyList<IFileHandler> FileHandlers => _fileHandlers;

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
