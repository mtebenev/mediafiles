using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace Mt.MediaMan.ClientApp.Cli
{
  [Command("open-catalog", Description = "Opens a catalog")]
  internal class ShellCommandOpenCatalog : ShellCommandBase
  {
    private readonly ShellAppContext _shellAppContext;

    public ShellCommandOpenCatalog(ShellAppContext shellAppContext)
    {
      _shellAppContext = shellAppContext;
    }

    [Argument(0, "catalogName")]
    public string CatalogName { get; set; }

    protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
    {
      await _shellAppContext.OpenCatalog(CatalogName);
      return Program.CommandResultContinue;
    }
  }
}
