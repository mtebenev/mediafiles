using System.Threading.Tasks;
using Mt.MediaFiles.AppEngine.CatalogStorage;

namespace Mt.MediaFiles.AppEngine.Scanning
{
  /// <summary>
  /// Scan sub-tasks are additional (and optional) tasks executed for scanned items.
  /// </summary>
  internal interface IScanTask
  {
    /// <summary>
    /// Unique id of the subtask.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Executes the sub-task on the catalog item record.
    /// </summary>
    Task ExecuteAsync(IScanContext scanContext, CatalogItemRecord record);
  }
}
