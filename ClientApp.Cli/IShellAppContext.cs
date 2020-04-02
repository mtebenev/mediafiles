using McMaster.Extensions.CommandLineUtils;
using Mt.MediaMan.AppEngine.Cataloging;

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
    /// Get/set current item for navigation
    /// </summary>
    ICatalogItem CurrentItem { get; set; }

    /// <summary>
    /// The console instance.
    /// </summary>
    public IConsole Console { get; }
  }
}
