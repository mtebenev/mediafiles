using McMaster.Extensions.CommandLineUtils;
using Mt.MediaFiles.AppEngine.Tasks;

namespace Mt.MediaFiles.ClientApp.Cli.Ui
{
  /// <summary>
  /// Implements IProgressIndicator with console UI
  /// </summary>
  internal class ProgressIndicatorConsole : IProgressIndicator
  {
    private readonly IConsole _console;

    /// <summary>
    /// Ctor.
    /// </summary>
    public ProgressIndicatorConsole(IConsole console)
    {
      this._console = console;
    }

    /// <summary>
    /// IProgressIndicator.
    /// </summary>
    public IProgressOperation StartOperation(string status)
    {
      if(!string.IsNullOrWhiteSpace(status))
        this._console.WriteLine(status);

      return new ProgressOperationConsole(0);
    }
  }
}
