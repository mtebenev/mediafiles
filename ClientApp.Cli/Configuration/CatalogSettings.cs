using System.Collections.Generic;
using Mt.MediaMan.AppEngine.CatalogStorage;

namespace Mt.MediaMan.ClientApp.Cli.Configuration
{
  /// <summary>
  /// The catalog settings in appsettings.json
  /// </summary>
  public class CatalogSettings : ICatalogSettings
  {
    /// <summary>
    /// ICatalogSettings.
    /// </summary>
    public string CatalogName { get; set; }

    /// <summary>
    /// ICatalogSettings.
    /// </summary>
    public string ConnectionString { get; set; }

    /// <summary>
    /// ICatalogSettings.
    /// </summary>
    public Dictionary<string, string> MediaRoots { get; set; }
  }
}
