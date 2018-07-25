using System.Collections.Generic;
using Mt.MediaMan.AppEngine.FileHandlers;

namespace Mt.MediaMan.AppEngine.Scanning
{
  internal class ScanConfiguration
  {
    public string ScanRootItemName { get; }
    private readonly List<IFileHandler> _fileHandlers;

    public ScanConfiguration(string scanRootItemName)
    {
      ScanRootItemName = scanRootItemName;
      _fileHandlers = new List<IFileHandler>
      {
        new FileHandlerVideo(),
        new FileHandlerEpub()
      };
    }

    public IReadOnlyList<IFileHandler> FileHandlers => _fileHandlers;
  }
}
