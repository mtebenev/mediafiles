using System;
using System.Threading.Tasks;

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

    /// <summary>
    /// To be removed when switched to another progress bar.
    /// </summary>
    Task Finish();
  }
}
