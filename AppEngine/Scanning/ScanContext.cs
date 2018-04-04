using System.Collections.Generic;

namespace Mt.MediaMan.AppEngine.Scanning
{
  internal class ScanContext : IScanContext
  {
    private readonly IList<IScanDriver> _scanDrivers;

    public ScanContext()
    {
      _scanDrivers = new List<IScanDriver>
      {
        new ScanDriverVideo(),
        new ScanDriverEpub()
      };
    }

    /// <summary>
    /// IScanContext
    /// </summary>
    public IList<IScanDriver> ScanDrivers => _scanDrivers;
  }
}
