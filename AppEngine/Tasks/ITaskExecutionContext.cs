using System;
using Microsoft.Extensions.Logging;
using Mt.MediaMan.AppEngine.Tasks;

namespace Mt.MediaMan.AppEngine.Commands
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
