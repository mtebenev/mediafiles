using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaMan.AppEngine.Commands;

namespace Mt.MediaMan.ClientApp.Cli
{
  [Command("init-catalog", Description = "Initializes new catalog")]
  internal class ShellCommandInitCatalog : ShellCommandBase
  {
    private readonly ICommandExecutionContext _executionContext;

    public ShellCommandInitCatalog(ICommandExecutionContext executionContext)
    {
      _executionContext = executionContext;
    }

    protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
    {
      await _executionContext.Catalog.InitializeNewCatalogAsync();
      return 0;
    }

  }
}
