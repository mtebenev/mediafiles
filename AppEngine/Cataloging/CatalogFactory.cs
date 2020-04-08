using System;
using System.IO.Abstractions;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using Mt.MediaFiles.AppEngine.Common;
using Mt.MediaFiles.AppEngine.Search;
using Mt.MediaMan.AppEngine.CatalogStorage;

namespace Mt.MediaFiles.AppEngine.Cataloging
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
