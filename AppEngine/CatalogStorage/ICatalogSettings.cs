using System.Collections.Generic;

namespace Mt.MediaFiles.AppEngine.CatalogStorage
{
  /// <summary>
  /// The injectable catalog settings.
  /// </summary>
  public interface ICatalogSettings
  {
    /// <summary>
    /// The catalog name.
    /// </summary>
    public string CatalogName { get; }

    /// <summary>
    /// The connection string for the catalog.
    /// </summary>
    public string ConnectionString { get; }

    /// <summary>
    /// Alias -> Absolute path
    /// </summary>
    public Dictionary<string, string> MediaRoots { get; }
  }
}
