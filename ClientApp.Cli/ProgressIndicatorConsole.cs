using System;
using Mt.MediaMan.AppEngine.Commands;

namespace Mt.MediaMan.ClientApp.Cli
{
  /// <summary>
  /// Implements IProgressIndicator with console UI
  /// </summary>
  internal class ProgressIndicatorConsole : IProgressIndicator
  {
    public void WriteStatus(string status)
    {
      Console.WriteLine(status);
    }
  }
}
