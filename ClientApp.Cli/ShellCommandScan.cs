using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaMan.AppEngine.Commands;

namespace Mt.MediaMan.ClientApp.Cli
{
  /// <summary>
  /// Scans new files to catalog
  /// </summary>
  [Command(Description = "Scans files to catalog")]
  internal class ShellCommandScan : ShellCommandBase
  {
    /// <summary>
    /// Injected
    /// </summary>
    public Shell Parent { get; set; }

    protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
    {
      await Parent.InitializeAsync();

      var scanPath = @"C:\_films";
      var command = new CommandScanFiles();
      await command.Execute(Parent.ExecutionContext.Catalog, scanPath);

      return 0;
    }
  }
}
