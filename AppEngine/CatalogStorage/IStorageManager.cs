using System;
using System.Data;
using YesSql;

namespace Mt.MediaFiles.AppEngine.CatalogStorage
{
  /// <summary>
  /// Provides low-level storage access
  /// </summary>
  internal interface IStorageManager : IDisposable
  {
    IDbConnection DbConnection { get; }
    IStore Store { get; }
  }
}
