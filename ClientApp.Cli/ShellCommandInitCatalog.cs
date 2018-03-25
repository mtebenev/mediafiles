using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace Mt.MediaMan.ClientApp.Cli
{
  [Command(Description = "Initializes new catalog")]
  internal class ShellCommandInitCatalog : ShellCommandBase
  {
    /// <summary>
    /// Injected
    /// </summary>
    public Shell Parent { get; set; }

    protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
    {
      await Parent.ExecutionContext.Catalog.InitializeNewCatalogAsync();
      return 0;
    }

  }
}
