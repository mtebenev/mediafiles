using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.Tasks;
using Mt.MediaFiles.ClientApp.Cli.Configuration;

namespace Mt.MediaFiles.ClientApp.Cli
{
  /// <summary>
  /// Not sure if it's a bug but subcommand option values are not re-build if executed multiple time.
  /// Thus we are re-creating application each time and keep current catalog item in the context
  /// </summary>
  internal class ShellAppContext : IShellAppContext
  {
    private readonly IAppSettingsManager _appSettingsManager;
    private ICatalog _catalog;
    private IReporter _reporter;

    public ShellAppContext(IAppSettingsManager appSettingsManager, ICatalog catalog)
    {
      this._appSettingsManager = appSettingsManager;
      this._catalog = catalog;
      this.CurrentItem = catalog.RootItem;
      this._reporter = new ConsoleReporter(this.Console);
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
    public IReporter Reporter => this._reporter;

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
    /// Resets catalog data
    /// </summary>
    public async Task ResetCatalogStorage(IServiceProvider serviceProvider)
    {
      var catalogName = _catalog.CatalogName;
      var connectionString = _appSettingsManager.AppSettings.Catalogs[catalogName].ConnectionString;
      var task = new CatalogTaskResetStorage(catalogName, connectionString);
      await task.ExecuteAsync(this._catalog);
    }

    /// <summary>
    /// Updates the settings file.
    /// </summary>
    internal void UpdateSettings()
    {
      this._appSettingsManager.Update();
    }
  }
}
