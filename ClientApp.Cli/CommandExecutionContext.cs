using System;
using Microsoft.Extensions.Logging;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Commands;

namespace Mt.MediaMan.ClientApp.Cli
{
  public class CommandExecutionContext : ICommandExecutionContext, IDisposable
  {
    public CommandExecutionContext(Catalog catalog, IProgressIndicator progressIndicator, ILoggerFactory loggerFactory)
    {
      Catalog = catalog;
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
