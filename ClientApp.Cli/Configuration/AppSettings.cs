using System.Collections.Generic;

namespace Mt.MediaFiles.ClientApp.Cli.Configuration
{
  /// <summary>
  /// App settings.
  /// Design note: the settings object is mutable and can saved back to a settings file.
  /// We don't watch for futher changes in the settings assuming short-living nature of the application.
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
        this.ExperimentalMode = other.ExperimentalMode;
        this.StartupCatalog = other.StartupCatalog;
        this.Catalogs = new Dictionary<string, CatalogSettings>(
          other.Catalogs ?? new Dictionary<string, CatalogSettings>()
        );
      }
    }

    /// <summary>
    /// Dev-mode commands/options.
    /// </summary>
    public bool ExperimentalMode { get; set; }

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
