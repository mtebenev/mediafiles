using System;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Commands;

namespace Mt.MediaMan.ClientApp.Cli
{
  public class CommandExecutionContext : ICommandExecutionContext, IDisposable
  {
    public CommandExecutionContext(Catalog catalog, IProgressIndicator progressIndicator)
    {
      Catalog = catalog;
      ProgressIndicator = progressIndicator;
    }

    public IProgressIndicator ProgressIndicator { get; }
    public Catalog Catalog { get; }

    /// <summary>
    /// IDisposable
    /// </summary>
    public void Dispose()
    {
      Catalog?.Dispose();
    }
  }
}
