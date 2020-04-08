using System;

namespace Mt.MediaFiles.AppEngine.Tasks
{
  /// <summary>
  /// Operation scope
  /// </summary>
  public interface IProgressOperation : IDisposable
  {
    void UpdateStatus(string text);
  }
}
