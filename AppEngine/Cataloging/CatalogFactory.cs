using System;
using System.IO.Abstractions;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using Mt.MediaFiles.AppEngine.Common;
using Mt.MediaFiles.AppEngine.Search;

namespace Mt.MediaFiles.AppEngine.Cataloging
{
  /// <summary>
  /// The catalog factory.
  /// </summary>
  public interface ICatalogFactory
  {
    /// <summary>
    /// Note: using service provider to delay the storage manager initialization.
    /// </summary>
    public Task<ICatalog> OpenCatalogAsync(ICatalogSettings catalogSettings, StorageConfiguration storageConfiguration);
  }

  public class CatalogFactory : ICatalogFactory
  {
    private readonly IServiceProvider _serviceProvider;
    private readonly IClock _clock;
    private readonly IFileSystem _fileSystem;
    private readonly AppEngineSettings _appEngineSettings;

    public CatalogFactory(
      IServiceProvider serviceProvider,
      IClock clock,
      IFileSystem fileSystem,
      AppEngineSettings appEngineSettings
      )
    {
      this._serviceProvider = serviceProvider;
      this._clock = clock;
      this._fileSystem = fileSystem;
      this._appEngineSettings = appEngineSettings;
    }

    /// <summary>
    /// ICatalogFactory.
    /// </summary>
    public async Task<ICatalog> OpenCatalogAsync(ICatalogSettings catalogSettings, StorageConfiguration storageConfiguration)
    {
      var storageManager = this._serviceProvider.GetRequiredService<IStorageManager>();
      var indexManager = new LuceneIndexManager(this._clock, this._fileSystem, this._appEngineSettings);
      var catalog = new Catalog(catalogSettings.CatalogName, storageManager, indexManager, this._fileSystem);

      await catalog.OpenAsync(storageConfiguration);

      return catalog;
    }
  }
}
