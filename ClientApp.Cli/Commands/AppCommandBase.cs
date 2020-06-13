using Mt.MediaFiles.AppEngine.Cataloging;
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
  }
}
