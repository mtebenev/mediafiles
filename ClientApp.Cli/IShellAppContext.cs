using McMaster.Extensions.CommandLineUtils;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.ClientApp.Cli.Core;

namespace Mt.MediaMan.ClientApp.Cli
{
  /// <summary>
  /// The shell application context.
  /// </summary>
  internal interface IShellAppContext
  {
    /// <summary>
    /// The catalog.
    /// </summary>
    ICatalog Catalog { get; }

    /// <summary>
    /// The current catalog location.
    /// </summary>
    public ICurrentLocation CurrentLocation { get; }

    /// <summary>
    /// The console instance.
    /// </summary>
    public IConsole Console { get; }
  }
}
