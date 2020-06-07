using System.Data;
using YesSql;

namespace Mt.MediaFiles.AppEngine.CatalogStorage
{
  /// <summary>
  /// Provides low-level storage access.
  /// Note: instance of the interface owns the resource. Don't dispose them.
  /// </summary>
  internal interface IStorageManager
  {
    /// <summary>
    /// The 'shared' database connection. Should not be used in multi-threaded scenarios.
    /// </summary>
    IDbConnection DbConnection { get; }

    /// <summary>
    /// The store instance.
    /// </summary>
    IStore Store { get; }
  }
}
