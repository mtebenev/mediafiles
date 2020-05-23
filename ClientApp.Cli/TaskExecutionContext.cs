using System;
using Microsoft.Extensions.Logging;
using Mt.MediaFiles.AppEngine.Tasks;

namespace Mt.MediaFiles.ClientApp.Cli
{
  internal class TaskExecutionContext : ITaskExecutionContext
  {
    public TaskExecutionContext(IServiceProvider serviceProvider, IProgressIndicator progressIndicator, ILoggerFactory loggerFactory)
    {
      this.ServiceProvider = serviceProvider;
      this.ProgressIndicator = progressIndicator;
      this.LoggerFactory = loggerFactory;
    }

    public IServiceProvider ServiceProvider { get; }
    public IProgressIndicator ProgressIndicator { get; }
    public ILoggerFactory LoggerFactory { get; }
  }
}
