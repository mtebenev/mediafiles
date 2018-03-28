using System.Collections.Generic;

namespace Mt.MediaMan.AppEngine.Scanning
{
  interface IScanContext
  {
    /// <summary>
    /// Scan drivers configured for the scan session
    /// </summary>
    IList<IScanDriver> ScanDrivers { get; }
  }
}
