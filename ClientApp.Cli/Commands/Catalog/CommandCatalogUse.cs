using McMaster.Extensions.CommandLineUtils;
using Mt.MediaFiles.ClientApp.Cli.Configuration;
using System.Threading.Tasks;

namespace Mt.MediaFiles.ClientApp.Cli.Commands.Catalog
{
  /// <summary>
  /// The command switches the startup catalog.
  /// </summary>
  [Command("use", Description = "Changes the startup catalog.")]
  internal class CommandCatalogUse
  {
    [Argument(0, "catalogName", Description = @"The name of the catalog.")]
    public string CatalogName { get; set; }

    public async Task<int> OnExecute(AppSettings appSettings, IConsole console, ShellAppContext shellAppContext)
    {
      if(appSettings.Catalogs.ContainsKey(this.CatalogName))
      {
        appSettings.StartupCatalog = this.CatalogName;
        shellAppContext.UpdateSettings();
      }

      return Program.CommandExitResult;
    }

  }
}
