using Microsoft.Extensions.Logging;

namespace Mt.MediaFiles.AppEngine.Tasks
{
  /// <summary>
  /// Provides contextual information to tasks
  /// </summary>
  public interface ITaskExecutionContext
  {
    ILoggerFactory LoggerFactory { get; }

    /// <summary>
    /// Starts a new progress bar during the operation.
    /// </summary>
    IProgressOperation StartProgressOperation(int maxTicks);

    /// <summary>
    /// Prints informational message during the task execution.
    /// </summary>
    void UpdateStatus(string message);
  }
}
