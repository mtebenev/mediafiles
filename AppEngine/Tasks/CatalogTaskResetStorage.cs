using System.Threading.Tasks;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.CatalogStorage;

namespace Mt.MediaFiles.AppEngine.Tasks
{
  /// <summary>
  /// The task resets the catalog storage.
  /// </summary>
  public sealed class CatalogTaskResetStorage : IInternalCatalogTask
  {
    private readonly string _catalogName;
    private readonly string _connectionString;

    /// <summary>
    /// Ctor.
    /// </summary>
    public CatalogTaskResetStorage(string catalogName, string connectionString)
    {
      this._catalogName = catalogName;
      this._connectionString = connectionString;
    }

    public Task ExecuteAsync(ICatalog catalog)
    {
      return catalog.ExecuteTaskAsync(this);
    }

    /// <summary>
    /// IInternalCatalogTask.
    /// </summary>
    async Task IInternalCatalogTask.ExecuteAsync(Catalog catalog)
    {
      await StorageManager.ResetStorage(this._connectionString);
      catalog.IndexManager.DeleteIndex(this._catalogName);
    }
  }
}
