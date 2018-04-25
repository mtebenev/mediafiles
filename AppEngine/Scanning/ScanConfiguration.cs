using System.Collections.Generic;
using Mt.MediaMan.AppEngine.FileHandlers;

namespace Mt.MediaMan.AppEngine.Scanning
{
  internal class ScanConfiguration
  {
    private readonly List<IFileHandler> _fileHandlers;

    public ScanConfiguration()
    {
      _fileHandlers = new List<IFileHandler>
      {
        new FileHandlerFilePropsIndexer(),
        new FileHandlerVideo(),
        new FileHandlerEpub()
      };

    }

    public IReadOnlyList<IFileHandler> FileHandlers => _fileHandlers;
  }
}
