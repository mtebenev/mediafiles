using System;

namespace Mt.MediaMan.AppEngine.Commands
{
  /// <summary>
  /// Operation scope
  /// </summary>
  public interface IProgressOperation : IDisposable
  {
    void UpdateStatus(string text);
  }
}
