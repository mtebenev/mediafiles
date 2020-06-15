using McMaster.Extensions.CommandLineUtils;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.CatalogStorage;

namespace Mt.MediaFiles.ClientApp.Cli
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
    /// The catalog settings.
    /// </summary>
    ICatalogSettings CatalogSettings { get; }

    /// <summary>
    /// Get/set current item for navigation
    /// </summary>
    ICatalogItem CurrentItem { get; set; }

    /// <summary>
    /// The console instance.
    /// </summary>
    public IConsole Console { get; }

    /// <summary>
    /// The reporter instance.
    /// </summary>
    public IReporter Reporter { get; }
  }
}
