using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace Mt.MediaMan.ClientApp.Cli.Commands
{
  [Command("reset-catalog", Description = "Resets catalog storage")]
  internal class ShellCommandResetCatalog : ShellCommandBase
  {
    private readonly ShellAppContext _shellAppContext;

    public ShellCommandResetCatalog(ShellAppContext shellAppContext)
    {
      _shellAppContext = shellAppContext;
    }

    protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
    {
      await _shellAppContext.ResetCatalogStorage();
      return Program.CommandResultContinue;
    }
  }
}
