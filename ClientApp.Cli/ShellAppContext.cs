using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.CatalogStorage;
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
    private readonly ICatalog _catalog;
    private readonly ICatalogSettings _catalogSettings;
    private IReporter _reporter;

    public ShellAppContext(IAppSettingsManager appSettingsManager, ICatalog catalog, ICatalogSettings catalogSettings)
    {
      this._appSettingsManager = appSettingsManager;
      this._catalog = catalog;
      this._catalogSettings = catalogSettings;
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
    public ICatalog Catalog => this._catalog;

    /// <summary>
    /// IShellAppContext.
    /// </summary>
    public ICatalogSettings CatalogSettings => this._catalogSettings;

    /// <summary>
    /// Updates the settings file.
    /// </summary>
    internal void UpdateSettings()
    {
      this._appSettingsManager.Update();
    }
  }
}
