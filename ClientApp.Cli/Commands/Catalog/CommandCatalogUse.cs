using McMaster.Extensions.CommandLineUtils;
using Mt.MediaFiles.ClientApp.Cli.Configuration;
using System;
using System.IO.Abstractions;
using System.Threading.Tasks;

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
      AppSettings appSettings,
      ShellAppContext shellAppContext,
      IEnvironment environment,
      IFileSystem fileSystem)
    {
      if(!appSettings.Catalogs.ContainsKey(this.CatalogName))
      {
        if(!this.Create)
        {
          throw new InvalidOperationException($"Unknown catalog: {this.CatalogName}");
        }
        shellAppContext.Console.WriteLine($"Creating catalog: {this.CatalogName}");
        var catalogSettings = DefaultSettings.CreateCatalogSettings(this.CatalogName, environment, fileSystem);
        appSettings.Catalogs.Add(this.CatalogName, catalogSettings);
      }

      appSettings.StartupCatalog = this.CatalogName;
      shellAppContext.UpdateSettings();

      return Program.CommandExitResult;
    }

  }
}
