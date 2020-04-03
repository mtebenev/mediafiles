using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.CatalogStorage;

namespace Mt.MediaMan.AppEngine.Scanning
{
  /// <summary>
  /// Scan sub-tasks are additional (and optional) tasks executed for scanned items.
  /// </summary>
  internal interface ISubTask
  {
    /// <summary>
    /// Executes the sub-task on the catalog item record.
    /// </summary>
    Task ExecuteAsync(IScanContext scanContext, CatalogItemRecord record);
  }
}
