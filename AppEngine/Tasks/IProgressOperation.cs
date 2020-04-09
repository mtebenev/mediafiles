using System;

namespace Mt.MediaFiles.AppEngine.Tasks
{
  /// <summary>
  /// Operation scope.
  /// </summary>
  public interface IProgressOperation : IDisposable
  {
    /// <summary>
    /// Update max tick count.
    /// </summary>
    void SetMaxTicks(int maxTicks);

    /// <summary>
    /// Update status.
    /// </summary>
    void UpdateStatus(string text);

    /// <summary>
    /// Create child scope.
    /// </summary>
    IProgressOperation CreateChildOperation(int maxTicks);
  }
}
