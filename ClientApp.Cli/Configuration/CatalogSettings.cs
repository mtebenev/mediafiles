using System.Collections.Generic;
using Mt.MediaFiles.AppEngine.CatalogStorage;

namespace Mt.MediaFiles.ClientApp.Cli.Configuration
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
