using Microsoft.Extensions.Logging;
using Mt.MediaMan.AppEngine.Cataloging;

namespace Mt.MediaMan.AppEngine.Commands
{
  /// <summary>
  /// Provides contextual information to tasks
  /// </summary>
  public interface ICommandExecutionContext
  {
    IProgressIndicator ProgressIndicator { get; }
    ICatalog Catalog { get; }
    ILoggerFactory LoggerFactory { get; }
  }
}
