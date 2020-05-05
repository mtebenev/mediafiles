using System.Threading.Tasks;
using Mt.MediaFiles.AppEngine.CatalogStorage;

namespace Mt.MediaFiles.AppEngine.Scanning
{
  /// <summary>
  /// The scan services are performing various tasks during scanning.
  /// </summary>
  public interface IScanService
  {
    /// <summary>
    /// Unique id of the subtask.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Executes the sub-task on the catalog item record.
    /// </summary>
    Task ScanAsync(IScanServiceContext context, CatalogItemRecord record);

    /// <summary>
    /// Flushes buffered data when scan is completed.
    /// </summary>
    Task FlushAsync();
  }
}
