using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Ebooks;
using Mt.MediaMan.ClientApp.Cli.Configuration;

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
    public async Task<bool> OpenCatalog(string catalogName)
    {
      bool result = false;

      // Check if catalog with such name exists in configuration
      if(_appSettings.Catalogs.ContainsKey(catalogName))
      {
        // Open the new catalog
        var storageConfiguration = new StorageConfiguration();
        EbooksModule.CreateStorageConfiguration(storageConfiguration);

        var catalog = Catalog.CreateCatalog(catalogName, _appSettings.Catalogs[catalogName].ConnectionString);
        await catalog.OpenAsync(storageConfiguration);

        // Close current catalog
        _catalog?.Close();

        _catalog = catalog;
        CurrentItem = _catalog.RootItem;

        this.Console.WriteLine($"Opened catalog: {catalogName}");

        result = true;
      }

      return result;
    }

    /// <summary>
    /// Resets catalog data
    /// </summary>
    public async Task ResetCatalogStorage()
    {
      var catalogName = _catalog.CatalogName;

      // Close current catalog in the shell context
      CurrentItem = null;
      _catalog = null;

      // Reset storage
      await Catalog.ResetCatalogStorage(catalogName, _appSettings.Catalogs[catalogName].ConnectionString);
      await OpenCatalog(catalogName);
    }
  }
}
