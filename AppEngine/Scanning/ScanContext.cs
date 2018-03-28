using System.Collections.Generic;

namespace Mt.MediaMan.AppEngine.Scanning
{
  internal class ScanContext : IScanContext
  {
    private readonly IList<IScanDriver> _scanDrivers;

    public ScanContext()
    {
      _scanDrivers = new List<IScanDriver>();
      _scanDrivers.Add(new ScanDriverVideo());
    }

    /// <summary>
    /// IScanContext
    /// </summary>
    public IList<IScanDriver> ScanDrivers => _scanDrivers;
  }
}
