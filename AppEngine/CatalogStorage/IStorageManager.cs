using System;
using System.Data;
using YesSql;

namespace Mt.MediaMan.AppEngine.CatalogStorage
{
  /// <summary>
  /// Provides low-level storage access
  /// </summary>
  internal interface IStorageManager : IDisposable
  {
    IDbConnection DbConnection { get; }
    Store Store { get; }
  }
}
