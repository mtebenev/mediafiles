using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using System.Threading.Tasks;

namespace Mt.MediaFiles.ClientApp.Cli.Commands
{
  /// <summary>
  /// MF application interface.
  /// </summary>
  internal interface IMediaFilesApp
  {
    /// <summary>
    /// Opens the catalog.
    /// </summary>
    Task<ICatalog> OpenCatalogAsync();

    /// <summary>
    /// Returns the catalog settings.
    /// </summary>
    ICatalogSettings GetCatalogSettings();
  }
}
