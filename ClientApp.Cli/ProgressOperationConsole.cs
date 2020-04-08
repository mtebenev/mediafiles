using McMaster.Extensions.CommandLineUtils;
using Mt.MediaFiles.AppEngine.Tasks;
using Mt.MediaMan.ClientApp.Cli;

namespace Mt.MediaFiles.ClientApp.Cli
{
  internal class ProgressOperationConsole : IProgressOperation
  {
    private readonly ProgressBar _progressBar;

    public ProgressOperationConsole(IConsole console)
    {
      _progressBar = new ProgressBar(false, console);
    }

    public void Dispose()
    {
      _progressBar.Dispose();
    }

    public void UpdateStatus(string text)
    {
      _progressBar.SetOperationText(text);
    }
  }
}
