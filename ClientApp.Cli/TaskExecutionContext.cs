using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Mt.MediaFiles.AppEngine.Tasks;
using Mt.MediaFiles.ClientApp.Cli.Ui;

namespace Mt.MediaFiles.ClientApp.Cli
{
  /// <summary>
  /// Task execution context implementation.
  /// </summary>
  internal class TaskExecutionContext : ITaskExecutionContext
  {
    private readonly IReporter _reporter;

    /// <summary>
    /// Ctor.
    /// </summary>
    public TaskExecutionContext(ILoggerFactory loggerFactory, IReporter reporter)
    {
      this.LoggerFactory = loggerFactory;
      this._reporter = reporter;
    }

    /// <summary>
    /// ITaskExecutionContext.
    /// </summary>
    public ILoggerFactory LoggerFactory { get; }

    /// <summary>
    /// ITaskExecutionContext.
    /// </summary>
    public IProgressOperation StartProgressOperation(int maxTicks)
    {
      var operation = new ProgressOperationConsole(this._reporter, maxTicks);
      return operation;
    }

    /// <summary>
    /// ITaskExecutionContext.
    /// </summary>
    public void UpdateStatus(string message)
    {
      this._reporter.Output(message);
    }
  }
}
