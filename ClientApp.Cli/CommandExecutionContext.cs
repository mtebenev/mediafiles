using System;
using Microsoft.Extensions.Logging;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Commands;

namespace Mt.MediaMan.ClientApp.Cli
{
  internal class CommandExecutionContext : ICommandExecutionContext, IDisposable
  {
    public CommandExecutionContext(ShellAppContext shellAppContext, IProgressIndicator progressIndicator, ILoggerFactory loggerFactory)
    {
      Catalog = shellAppContext.Catalog;
      ProgressIndicator = progressIndicator;
      LoggerFactory = loggerFactory;
    }

    public IProgressIndicator ProgressIndicator { get; }
    public Catalog Catalog { get; }
    public ILoggerFactory LoggerFactory { get; }

    /// <summary>
    /// IDisposable
    /// </summary>
    public void Dispose()
    {
      Catalog?.Dispose();
    }
  }
}
