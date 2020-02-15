using System.Collections.Generic;

namespace Mt.MediaMan.ClientApp.Cli.Configuration
{
  /// <summary>
  /// Serialized app settings
  /// </summary>
  public class AppSettings
  {
    /// <summary>
    /// Catalog to open on startup
    /// </summary>
    public string StartupCatalog { get; set; }

    /// <summary>
    /// Catalog name -> connection string
    /// </summary>
    public Dictionary<string, CatalogSettings> Catalogs { get; set; }
  }
}
