using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using System.Threading.Tasks;

namespace Mt.MediaFiles.ClientApp.Cli.Commands
{
  /// <summary>
  /// Base class for commands.
  /// </summary>
  internal abstract class AppCommandBase
  {
    /// <summary>
    /// Injected app instance.
    /// </summary>
    public IMediaFilesApp Parent { get; set; }

    /// <summary>
    /// Opens the catalog.
    /// </summary>
    protected Task<ICatalog> OpenCatalogAsync()
    {
      return this.Parent.OpenCatalogAsync();
    }

    /// <summary>
    /// Returns the catalog settings.
    /// </summary>
    protected ICatalogSettings GetCatalogSetings()
    {
      return this.Parent.GetCatalogSettings();
    }
  }
}
