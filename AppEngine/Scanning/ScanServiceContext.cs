using Microsoft.Extensions.Logging;
using Mt.MediaFiles.AppEngine.Tasks;

namespace Mt.MediaFiles.AppEngine.Scanning
{
  /// <summary>
  /// Scan service context implementation.
  /// </summary>
  internal class ScanServiceContext : IScanServiceContext
  {
    /// <summary>
    /// Ctor.
    /// </summary>
    public ScanServiceContext(IScanContext scanContext)
    {
      this.Logger = scanContext.Logger;
      this.ProgressOperation = scanContext.ProgressOperation;
    }

    /// <summary>
    /// IScanServiceContext.
    /// </summary>
    public ILogger Logger { get; }

    /// <summary>
    /// IScanServiceContext.
    /// </summary>
    public IProgressOperation ProgressOperation { get; }
  }
}
