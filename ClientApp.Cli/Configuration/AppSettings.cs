using System.Collections.Generic;

namespace Mt.MediaMan.ClientApp.Cli.Configuration
{
  /// <summary>
  /// Serialized app settings
  /// </summary>
  public class AppSettings
  {
    /// <summary>
    /// Ctor.
    /// </summary>
    public AppSettings()
    {
    }

    /// <summary>
    /// Copy ctor.
    /// </summary>
    public AppSettings(AppSettings other)
    {
      if(other != null)
      {
        this.StartupCatalog = other.StartupCatalog;
        this.Catalogs = new Dictionary<string, CatalogSettings>(
          other.Catalogs ?? new Dictionary<string, CatalogSettings>()
        );
      }
    }

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
