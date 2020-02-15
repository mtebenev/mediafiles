using System.Collections.Generic;

namespace Mt.MediaMan.ClientApp.Cli.Configuration
{
  /// <summary>
  /// The catalog settings in appsettings.json
  /// </summary>
  public class CatalogSettings
  {
    /// <summary>
    /// The connection string for the catalog.
    /// </summary>
    public string ConnectionString { get; set; }

    /// <summary>
    /// Alias -> Absolute path
    /// </summary>
    public Dictionary<string, string> MediaRoots { get; set; }
  }
}
