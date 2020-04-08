using System;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaFiles.AppEngine.Tasks;

namespace Mt.MediaFiles.ClientApp.Cli
{
  /// <summary>
  /// Implements IProgressIndicator with console UI
  /// </summary>
  internal class ProgressIndicatorConsole : IProgressIndicator
  {
    private readonly IConsole _console;

    public ProgressIndicatorConsole(IConsole console)
    {
      _console = console;
    }

    public IProgressOperation StartOperation(string status)
    {
      if(!string.IsNullOrWhiteSpace(status))
        _console.WriteLine(status);

      return new ProgressOperationConsole(_console);
    }
  }
}
