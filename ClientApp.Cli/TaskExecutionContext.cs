using System;
using Microsoft.Extensions.Logging;
using Mt.MediaMan.AppEngine.Commands;
using Mt.MediaMan.AppEngine.Tasks;

namespace Mt.MediaMan.ClientApp.Cli
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
