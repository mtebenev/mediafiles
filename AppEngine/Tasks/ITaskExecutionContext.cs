using System;
using Microsoft.Extensions.Logging;

namespace Mt.MediaFiles.AppEngine.Tasks
{
  /// <summary>
  /// Provides contextual information to tasks
  /// </summary>
  public interface ITaskExecutionContext
  {
    IServiceProvider ServiceProvider { get; }
    IProgressIndicator ProgressIndicator { get; }
    ILoggerFactory LoggerFactory { get; }
  }
}
