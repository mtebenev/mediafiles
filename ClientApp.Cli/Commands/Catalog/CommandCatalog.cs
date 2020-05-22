using McMaster.Extensions.CommandLineUtils;

namespace Mt.MediaFiles.ClientApp.Cli.Commands.Catalog
{
  [Command("catalog", Description = "Catalog configuration commands.")]
  [Subcommand(
    typeof(Commands.Catalog.CommandCatalogList),
    typeof(Commands.Catalog.CommandCatalogUse))]
  internal class CommandCatalog
  {
    public int OnExecute(CommandLineApplication app, IConsole console)
    {
      app.ShowHelp();
      return Program.CommandExitResult;
    }
  }
}
