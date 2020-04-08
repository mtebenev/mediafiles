using Microsoft.Extensions.Logging;
using Mt.MediaFiles.AppEngine.Tasks;

namespace Mt.MediaFiles.AppEngine.Scanning
{
  /// <summary>
  /// The context for scan services execution.
  /// </summary>
  public interface IScanServiceContext
  {
    ILogger Logger { get; }
    IProgressOperation ProgressOperation { get; }
  }
}
