using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace Mt.MediaMan.ClientApp.Cli.Commands
{
  [Command("reset-catalog", Description = "Resets catalog storage")]
  internal class ShellCommandResetCatalog : ShellCommandBase
  {
    public async Task<int> OnExecuteAsync(ShellAppContext shellAppContext)
    {
      await shellAppContext.ResetCatalogStorage();
      return Program.CommandResultContinue;
    }
  }
}
