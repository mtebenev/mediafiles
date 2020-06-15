using McMaster.Extensions.CommandLineUtils;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using System.Threading.Tasks;

namespace Mt.MediaFiles.ClientApp.Cli.Commands.Catalog
{
  [Command("catalog", Description = "Catalog configuration commands.")]
  [Subcommand(
    typeof(Commands.Catalog.CommandCatalogList),
    typeof(Commands.Catalog.CommandCatalogReset),
    typeof(Commands.Catalog.CommandCatalogUse))]
  internal class CommandCatalog :
    AppCommandBase,
    IMediaFilesApp
  {
    public int OnExecute(CommandLineApplication app)
    {
      app.ShowHelp();
      return Program.CommandExitResult;
    }

    /// <summary>
    /// IMediaFilesApp.
    /// </summary>
    public ICatalogSettings GetCatalogSettings()
    {
      return this.Parent.GetCatalogSettings();
    }

    /// <summary>
    /// IMediaFilesApp.
    /// </summary>
    Task<ICatalog> IMediaFilesApp.OpenCatalogAsync()
    {
      return this.Parent.OpenCatalogAsync();
    }
  }
}
