using System;
using System.Threading.Tasks;
using AppEngine.Video.VideoImprint;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.Tasks;
using Mt.MediaMan.AppEngine.Ebooks;
using Mt.MediaMan.ClientApp.Cli.Configuration;

namespace Mt.MediaFiles.ClientApp.Cli
{
  /// <summary>
  /// Not sure if it's a bug but subcommand option values are not re-build if executed multiple time.
  /// Thus we are re-creating application each time and keep current catalog item in the context
  /// </summary>
  internal class ShellAppContext : IShellAppContext
  {
    private readonly AppSettings _appSettings;
    private ICatalog _catalog;

    public ShellAppContext(AppSettings appSettings)
    {
      this._appSettings = appSettings;
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
    /// Opens catalog
    /// </summary>
    public async Task OpenCatalog(IServiceProvider serviceProvider)
    {
      // Open the new catalog
      var storageConfiguration = new StorageConfiguration();
      EbooksModule.CreateStorageConfiguration(storageConfiguration);
      VideoImprintModule.ConfigureStorage(storageConfiguration);

      var catalog = await CatalogFactory.OpenCatalogAsync(serviceProvider, storageConfiguration);

      // Close current catalog
      this._catalog?.Close();

      this._catalog = catalog;
      this.CurrentItem = this._catalog.RootItem;

      this.Console.WriteLine($"Opened catalog: {this.Catalog.CatalogName}");
    }

    /// <summary>
    /// Resets catalog data
    /// TODO: For now just quit the app after the catalog reset. Cannot re-open the catalog.
    /// </summary>
    public async Task ResetCatalogStorage(IServiceProvider serviceProvider)
    {
      var catalogName = _catalog.CatalogName;

      // Reset storage
      var task = new CatalogTaskResetStorage(catalogName, _appSettings.Catalogs[catalogName].ConnectionString);
      await task.ExecuteAsync(this._catalog);

      // Re-open the current catalog
      await this.OpenCatalog(serviceProvider);
    }
  }
}
