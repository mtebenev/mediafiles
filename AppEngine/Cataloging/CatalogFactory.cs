using System;
using System.IO.Abstractions;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mt.MediaMan.AppEngine.CatalogStorage;
using Mt.MediaMan.AppEngine.Common;
using Mt.MediaMan.AppEngine.Search;

namespace Mt.MediaMan.AppEngine.Cataloging
{
  /// <summary>
  /// The catalog factory.
  /// </summary>
  public static class CatalogFactory
  {
    public static async Task<ICatalog> OpenCatalogAsync(IServiceProvider serviceProvider, StorageConfiguration storageConfiguration)
    {
      var catalogSettings = serviceProvider.GetRequiredService<ICatalogSettings>();
      var storageManager = serviceProvider.GetRequiredService<IStorageManager>();
      var clock = serviceProvider.GetRequiredService<IClock>();
      var fileSystem = serviceProvider.GetRequiredService<IFileSystem>();

      var indexManager = new LuceneIndexManager(clock);
      var catalog = new Catalog(catalogSettings.CatalogName, storageManager, indexManager, fileSystem);

      await catalog.OpenAsync(storageConfiguration);

      return catalog;
    }
  }
}
