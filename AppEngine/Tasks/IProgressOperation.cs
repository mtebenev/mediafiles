using System;

namespace Mt.MediaMan.AppEngine.Tasks
{
  /// <summary>
  /// Operation scope
  /// </summary>
  public interface IProgressOperation : IDisposable
  {
    void UpdateStatus(string text);
  }
}
