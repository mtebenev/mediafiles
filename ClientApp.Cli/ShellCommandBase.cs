using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace Mt.MediaMan.ClientApp.Cli
{
  [HelpOption("--help")]
  public abstract class ShellCommandBase
  {
    /// <summary>
    /// Should return 0 to continue execution
    /// </summary>
    protected abstract Task<int> OnExecuteAsync(CommandLineApplication app);
  }
}
