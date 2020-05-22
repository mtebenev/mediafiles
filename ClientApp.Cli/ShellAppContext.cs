using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.Ebooks;
using Mt.MediaFiles.AppEngine.Tasks;
using Mt.MediaFiles.AppEngine.Video.Thumbnail;
using Mt.MediaFiles.AppEngine.Video.VideoImprint;
using Mt.MediaFiles.ClientApp.Cli.Configuration;

namespace Mt.MediaFiles.ClientApp.Cli
{
  /// <summary>
  /// Not sure if it's a bug but subcommand option values are not re-build if executed multiple time.
  /// Thus we are re-creating application each time and keep current catalog item in the context
  /// </summary>
  internal class ShellAppContext : IShellAppContext
  {
    private readonly AppSettingsManager _appSettingsManager;
    private ICatalog _catalog;

    public ShellAppContext(AppSettingsManager appSettingsManager)
    {
      this._appSettingsManager = appSettingsManager;
      this._catalog = null;
      this.CurrentItem = null;
    }

    /// <summary>
    /// IShellAppContext.
    /// </summary>
    public ICatalogItem CurrentItem { get; set; }

    /// <summary>
    /// IShellAppContext.
    /// </summary>
    public IConsole Console => PhysicalConsole.Singleton;

    /// <summary>
    /// IShellAppContext.
    /// </summary>
    public ICatalog Catalog
    {
      get
      {
        if(this._catalog == null)
          throw new InvalidOperationException("Catalog is not open");

        return this._catalog;
      }
    }

    /// <summary>
    /// Opens a catalog.
    /// </summary>
    public async Task OpenCatalog(IServiceProvider serviceProvider)
    {
      // Open the new catalog
      var storageConfiguration = new StorageConfiguration();
      EbooksModule.CreateStorageConfiguration(storageConfiguration);
      VideoImprintModule.ConfigureStorage(storageConfiguration);
      ThumbnailModule.ConfigureStorage(storageConfiguration);

      var catalog = await CatalogFactory.OpenCatalogAsync(serviceProvider, storageConfiguration);

      // Close current catalog
      this._catalog?.Close();

      this._catalog = catalog;
      this.CurrentItem = this._catalog.RootItem;

      this.Console.WriteLine($"Opened catalog: {this.Catalog.CatalogName}");
    }

    /// <summary>
    /// Resets catalog data
    /// </summary>
    public async Task ResetCatalogStorage(IServiceProvider serviceProvider)
    {
      var catalogName = _catalog.CatalogName;
      var connectionString = _appSettingsManager.AppSettings.Catalogs[catalogName].ConnectionString;
      var task = new CatalogTaskResetStorage(catalogName, connectionString);
      await task.ExecuteAsync(this._catalog);
    }
  }
}
