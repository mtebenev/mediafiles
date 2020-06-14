using McMaster.Extensions.CommandLineUtils;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using Mt.MediaFiles.AppEngine.Ebooks;
using Mt.MediaFiles.AppEngine.Video.Thumbnail;
using Mt.MediaFiles.AppEngine.Video.VideoImprint;
using Mt.MediaFiles.ClientApp.Cli.Commands.Shell;
using Mt.MediaFiles.ClientApp.Cli.Configuration;
using Mt.MediaFiles.ClientApp.Cli.Core;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Mt.MediaFiles.ClientApp.Cli.Commands
{
  /// <summary>
  /// The root command.
  /// </summary>
  [Command(
    "mf",
    FullName = "mediafiles",
    Description = "Media files cataloging software.")]
  [Subcommand(
    typeof(CommandShell),
    typeof(Commands.CommandCheckStatus),
    typeof(Commands.CommandResetCatalog),
    typeof(Commands.CommandScan),
    typeof(Commands.CommandSearchVideo),
    typeof(Commands.CommandSearchVideoDuplicates),
    typeof(Commands.CommandServe),
    typeof(Commands.CommandUpdate),
    typeof(Commands.Catalog.CommandCatalog))]
  [VersionOptionFromMember("--version", MemberName = nameof(GetVersion))]
  internal class CommandMediaFiles : IMediaFilesApp
  {
    private readonly IServiceProvider _serviceProvider;
    private readonly IConsole _console;
    private readonly ICatalogFactory _catalogFactory;
    private readonly IDbConnectionSource _dbConnectionSource;
    private readonly AppSettings _appSettings;

    /// <summary>
    /// Ctor.
    /// </summary>
    public CommandMediaFiles(
      IServiceProvider serviceProvider,
      IConsole console,
      ICatalogFactory catalogFactory,
      IDbConnectionSource dbConnectionSource,
      AppSettings appSettings)
    {
      this._serviceProvider = serviceProvider;
      this._console = console;
      this._catalogFactory = catalogFactory;
      this._dbConnectionSource = dbConnectionSource;
      this._appSettings = appSettings;
    }

    [Option(LongName = "catalog", ShortName = "c", Description = "Catalog name.")]
    public string CatalogName { get; set; }

    /// <summary>
    /// Invoked only if none command could be found (the default command).
    /// </summary>
    public Task<int> OnExecuteAsync(CommandLineApplication app, AppSettings appSettings)
    {
      Task<int> result;
      if(appSettings.ExperimentalMode)
        result = app.ExecuteAsync(new[] { "shell" });
      else
      {
        app.ShowHelp();
        return Task.FromResult(Program.CommandExitResult);
      }

      return result;
    }

    public async Task<ICatalog> OpenCatalogAsync()
    {
      var catalogSettings = this.GetCatalogSettings();

      // Storage configuration (init external storage modules)
      var storageConfiguration = new StorageConfiguration();
      EbooksModule.CreateStorageConfiguration(storageConfiguration);
      VideoImprintModule.ConfigureStorage(storageConfiguration);
      ThumbnailModule.ConfigureStorage(storageConfiguration);
      this._dbConnectionSource.SetConnectionString(catalogSettings.ConnectionString);

      // Open the catalog (default or contextual)
      var catalog = await this._catalogFactory.OpenCatalogAsync(catalogSettings, storageConfiguration);
      this._console.WriteLine($"Using catalog: {catalog.CatalogName}");

      return catalog;
    }

    /// <summary>
    /// IMediaFilesApp.
    /// </summary>
    public ICatalogSettings GetCatalogSettings()
    {
      if(!string.IsNullOrEmpty(this.CatalogName) && !this._appSettings.Catalogs.ContainsKey(this.CatalogName))
      {
        throw new InvalidOperationException($"The catalog with name '{this.CatalogName}' is unknown. Please check the command line and configuration file.");
      }

      var catalogSettings = this._appSettings.Catalogs[
        string.IsNullOrEmpty(this.CatalogName)
        ? this._appSettings.StartupCatalog
        : this.CatalogName];

      return catalogSettings;
    }

    /// <summary>
    /// The version retriever.
    /// </summary>
    private static string GetVersion()
        => typeof(Program).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
  }
}
