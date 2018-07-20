using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaMan.AppEngine.Commands;

namespace Mt.MediaMan.ClientApp.Cli
{
  /// <summary>
  /// Finds duplicate items in catalog
  /// </summary>
  [Command("find-duplicates", Description = "Finds duplicate items in the catalog")]
  internal class ShellCommandFindDuplicates : ShellCommandBase
  {
    private readonly ICommandExecutionContext _executionContext;
    private readonly ShellAppContext _shellAppContext;

    public ShellCommandFindDuplicates(ICommandExecutionContext executionContext, ShellAppContext shellAppContext)
    {
      _executionContext = executionContext;
      _shellAppContext = shellAppContext;
    }

    protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
    {
      var command = new CommandFindDuplicates();
      var result = await command.Execute(_executionContext.Catalog);

      _shellAppContext.Console.WriteLine($"{result.Count} duplicates found:");
      var index = 1;
      foreach (var duplicates in result)
      {
        var firstItem = await _executionContext.Catalog.GetItemByIdAsync(duplicates.CatalogItemIds[0]);
        _shellAppContext.Console.WriteLine($"{index}: {firstItem.Name}");

        index++;
      }

      return 0;
    }
  }
}
