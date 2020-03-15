using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.CatalogStorage;
using Mt.MediaMan.AppEngine.Common;
using Mt.MediaMan.AppEngine.Search;

namespace Mt.MediaMan.AppEngine.Tasks
{
  /// <summary>
  /// The task resets the catalog storage.
  /// </summary>
  public class CatalogTaskResetStorage : ICatalogTask
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

    /// <summary>
    /// ICatalogTask
    /// </summary>
    async Task ICatalogTask.ExecuteAsync(Catalog catalog)
    {
      await StorageManager.ResetStorage(this._connectionString);
      var indexManager = new LuceneIndexManager(new Clock());
      indexManager.DeleteIndex(this._catalogName);
    }
  }
}
