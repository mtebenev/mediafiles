using McMaster.Extensions.CommandLineUtils;
using Mt.MediaFiles.ClientApp.Cli.Configuration;

namespace Mt.MediaFiles.ClientApp.Cli.Commands.Catalog
{
  [Command("list", Description = "Lists registered catalogs.")]
  internal class CommandCatalogList
  {
    public int OnExecute(AppSettings appSettings, IConsole console)
    {
      console.WriteLine("Registered catalogs:");
      foreach (var c in appSettings.Catalogs)
      {
        console.WriteLine(c.Key);
      }

      return Program.CommandExitResult;
    }
  }
}
