using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Ebooks;

namespace Mt.MediaMan.ClientApp.Cli
{
  /// <summary>
  /// Not sure if it's a bug but subcommand option values are not re-build if executed multiple time.
  /// Thus we are re-creating application each time and keep current catalog item in the context
  /// </summary>
  internal class ShellAppContext
  {
    private readonly AppSettings _appSettings;
    private Catalog _catalog;

    public ShellAppContext(AppSettings appSettings)
    {
      _appSettings = appSettings;
      CurrentItem = null;
    }

    /// <summary>
    /// Get/set current item for navigation
    /// </summary>
    public ICatalogItem CurrentItem { get; set; }

    public IConsole Console => PhysicalConsole.Singleton;

    public Catalog Catalog
    {
      get
      {
        if(_catalog == null)
          throw new InvalidOperationException("Catalog is not open");

        return _catalog;
      }
    }

    /// <summary>
    /// Opens catalog
    /// </summary>
    public async Task OpenCatalog()
    {
      if(_catalog != null)
        throw new InvalidOperationException("Catalog is already open");

      var storageConfiguration = new StorageConfiguration();
      EbooksModule.CreateStorageConfiguration(storageConfiguration);

      // Open catalog
      _catalog = Catalog.CreateCatalog(_appSettings.ConnectionString);
      await _catalog.OpenAsync(storageConfiguration);
      CurrentItem = _catalog.RootItem;
    }

    /// <summary>
    /// Resets catalog data
    /// </summary>
    public async Task ResetCatalogStorage()
    {
      // Close current catalog in the shell context
      CurrentItem = null;
      _catalog = null;

      // Reset storage
      await Catalog.ResetCatalogStorage(_appSettings.ConnectionString);
      await OpenCatalog();
    }
  }
}
