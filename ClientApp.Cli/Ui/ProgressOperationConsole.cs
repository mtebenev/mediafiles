using Luna.ConsoleProgressBar;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaFiles.AppEngine.Tasks;

namespace Mt.MediaFiles.ClientApp.Cli.Ui
{
  /// <summary>
  /// Wraps ShellProgressBar
  /// </summary>
  internal class ProgressOperationConsole : IProgressOperation
  {
    private readonly ConsoleProgressBar _progressBar;
    private readonly IReporter _reporter;
    private readonly double _maxTicks;
    private int _currentTick;

    /// <summary>
    /// Ctor for root operations.
    /// </summary>
    public ProgressOperationConsole(IReporter reporter, int maxTicks)
    {
      this._progressBar =
        new ConsoleProgressBar
        {
          NumberOfBlocks = 100
        };
      this._maxTicks = maxTicks;
      this._currentTick = 0;
      this._reporter = reporter;
    }

    /// <summary>
    /// IProgressOperation.
    /// </summary>
    public void Tick()
    {
      this._currentTick++;
      this._progressBar.Report(this._currentTick / this._maxTicks);
    }

    /// <summary>
    /// IProgressOperation.
    /// </summary>
    public void UpdateStatus(string text)
    {
      this._reporter.Output(text);
    }

    /// <summary>
    /// IDisposable.
    /// </summary>
    public void Dispose()
    {
      this._progressBar.Dispose();
      this._reporter.Output("");
    }
  }
}
