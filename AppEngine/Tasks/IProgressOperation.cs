using System;

namespace Mt.MediaFiles.AppEngine.Tasks
{
  /// <summary>
  /// Operation scope.
  /// </summary>
  public interface IProgressOperation : IDisposable
  {
    /// <summary>
    /// Update progress.
    /// </summary>
    void Tick();
  }
}
