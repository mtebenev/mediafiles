using System.Collections.Generic;

namespace Mt.MediaFiles.AppEngine.Scanning
{
  /// <summary>
  /// Encapsulates logic for a scan service instantiation.
  /// </summary>
  public interface IScanServiceFactory
  {
    /// <summary>
    /// Unique id of the service.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Dependency IDs.
    /// </summary>
    IReadOnlyList<string> Dependencies { get; }

    /// <summary>
    /// Instantiates the scan service object.
    /// </summary>
    IScanService Create();
  }
}
