using System;
using Mt.MediaFiles.AppEngine.Tasks;
using ShellProgressBar;

namespace Mt.MediaFiles.ClientApp.Cli.Ui
{
  /// <summary>
  /// Wraps ShellProgressBar
  /// </summary>
  internal class ProgressOperationConsole : IProgressOperation
  {
    private readonly IProgressBar _progressBar;
    private readonly bool _clearOnDispose;

    /// <summary>
    /// Ctor for root operations.
    /// </summary>
    public ProgressOperationConsole(int maxTicks)
    {
      var options = this.CreateDefaultOptions();
      this._progressBar = new ProgressBar(maxTicks, "", options);
      this._clearOnDispose = true;
    }

    /// <summary>
    /// Ctor for child operations.
    /// </summary>
    public ProgressOperationConsole(ChildProgressBar childProgressBar)
    {
      this._progressBar = childProgressBar;
      this._clearOnDispose = false;
    }

    /// <summary>
    /// IProgressOperation.
    /// </summary>
    public IProgressOperation CreateChildOperation(int maxTicks)
    {
      var options = this.CreateDefaultOptions();
      var childBar = this._progressBar.Spawn(maxTicks, "", options);
      var childProgress = new ProgressOperationConsole(childBar);

      return childProgress;
    }

    /// <summary>
    /// IProgressOperation.
    /// </summary>
    public void UpdateStatus(string text)
    {
      this._progressBar.Tick(text);
    }

    /// <summary>
    /// IProgressOperation.
    /// </summary>
    public void SetMaxTicks(int maxTicks)
    {
      this._progressBar.MaxTicks = maxTicks;
    }

    /// <summary>
    /// IDisposable.
    /// </summary>
    public void Dispose()
    {
      // This is workaround for ShellProgressBar - it keeps the root progress on the screen.
      if(this._clearOnDispose)
      {
        this._progressBar.ForegroundColor = Console.BackgroundColor;
        this._progressBar.WriteLine(" ");
      }

      this._progressBar.Dispose();
    }

    private ProgressBarOptions CreateDefaultOptions()
    {
      var result = new ProgressBarOptions
      {
        ProgressCharacter = '─',
        CollapseWhenFinished = true,
        BackgroundColor = ConsoleColor.Gray,
        ForegroundColorDone = ConsoleColor.DarkGray,
        ForegroundColor = ConsoleColor.White,
      };

      return result;
    }
  }
}
