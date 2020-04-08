using System.Collections.Generic;
using System.Threading.Tasks;
using Mt.MediaFiles.AppEngine.Cataloging;

namespace Mt.MediaFiles.AppEngine.Tasks
{
  /// <summary>
  /// Performs search for files in catalog.
  /// The query is the file name mask
  /// </summary>
  public sealed class CatalogTaskSearchFiles : IInternalCatalogTask<IList<int>>
  {
    private readonly string _query;

    /// <summary>
    /// Ctor.
    /// </summary>
    public CatalogTaskSearchFiles(string query)
    {
      this._query = query;
    }

    public Task<IList<int>> ExecuteAsync(ICatalog catalog)
    {
      return catalog.ExecuteTaskAsync(this);
    }

    /// <summary>
    /// IInternalCatalogTask.
    /// </summary>
    Task<IList<int>> IInternalCatalogTask<IList<int>>.ExecuteAsync(Catalog catalog)
    {
      return catalog.ItemStorage.SearchItemsAsync(this._query);
    }
  }
}
