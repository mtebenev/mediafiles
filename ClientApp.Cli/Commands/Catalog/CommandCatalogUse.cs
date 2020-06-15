using McMaster.Extensions.CommandLineUtils;
using Mt.MediaFiles.ClientApp.Cli.Configuration;
using System;
using System.IO.Abstractions;

namespace Mt.MediaFiles.ClientApp.Cli.Commands.Catalog
{
  /// <summary>
  /// The command switches the startup catalog.
  /// </summary>
  [Command("use", Description = "Changes the startup catalog.")]
  internal class CommandCatalogUse
  {
    [Argument(0, "catalogName", Description = @"The name of the catalog.")]
    public string CatalogName { get; set; }

    [Option(ShortName = "c", LongName = "create", Description = "Forces the new catalog creation.")]
    public bool Create { get; set; }

    public int OnExecute(
      IAppSettingsManager appSettingsManager,
      IConsole console,
      IEnvironment environment,
      IFileSystem fileSystem)
    {
      var appSettings = appSettingsManager.AppSettings;
      if(!appSettings.Catalogs.ContainsKey(this.CatalogName))
      {
        if(!this.Create)
        {
          throw new InvalidOperationException($"Unknown catalog: {this.CatalogName}");
        }
        console.WriteLine($"Creating catalog: {this.CatalogName}");
        var catalogSettings = DefaultSettings.CreateCatalogSettings(this.CatalogName, environment, fileSystem);
        appSettings.Catalogs.Add(this.CatalogName, catalogSettings);
      }

      appSettings.StartupCatalog = this.CatalogName;
      appSettingsManager.Update();
      console.WriteLine($"Startup catalog changed to: {this.CatalogName}");

      return Program.CommandExitResult;
    }

  }
}
