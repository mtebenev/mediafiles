using System;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using AppEngine.Video.VideoImprint;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mt.MediaMan.AppEngine;
using Mt.MediaMan.AppEngine.CatalogStorage;
using Mt.MediaMan.AppEngine.Commands;
using Mt.MediaMan.ClientApp.Cli.Commands;
using Mt.MediaMan.ClientApp.Cli.Configuration;
using NLog.Extensions.Logging;
using MediaToolkit;
using AppEngine.Video.Test;
using Mt.MediaMan.AppEngine.Tasks;
using Mt.MediaMan.AppEngine.Common;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.Sqlite;

namespace Mt.MediaMan.ClientApp.Cli
{
  [Command("mediaman")]
  [Subcommand(
    typeof(Shell),
    typeof(Commands.CommandCheckStatus),
    typeof(Commands.CommandUpdate))]
  internal class Program
  {
    public const int CommandExitResult = -1;
    public const int CommandResultContinue = 0;

    private static IServiceProvider _services;
    private static ShellAppContext _shellAppContext;

    public static async Task<int> Main(string[] args)
    {
      int result = 0;
      try
      {
        Console.SetWindowSize(220, 54);
        NLog.LogManager.LoadConfiguration("nlog.config");

        var configuration = new ConfigurationBuilder()
          .SetBasePath(AppContext.BaseDirectory)
          .AddJsonFile("appsettings.json", true)
          .Build();

        var appSettings = configuration.Get<AppSettings>();
        if(appSettings == null)
          appSettings = Program.CreateDefaultSettings();

        Program._shellAppContext = new ShellAppContext(appSettings);

        // Open startup or first catalog
        if(appSettings.Catalogs.Count == 0)
          throw new InvalidOperationException("No catalogs defined");

        Program._services = Program.ConfigureServices(appSettings);
        await _shellAppContext.OpenCatalog(Program._services);

        var app = new CommandLineApplication<Program>();
        app.Conventions
          .UseDefaultConventions()
          .UseConstructorInjection(Program._services);

        result = await app.ExecuteAsync(args);
      }
      finally
      {
        Program._shellAppContext?.Catalog.Dispose();
      }
      return result;
    }

    /// <summary>
    /// Invoked only if none command could be found (the default command).
    /// </summary>
    public Task<int> OnExecuteAsync(CommandLineApplication app, IConsole console)
    {
      return app.ExecuteAsync(new[] { "shell" });
    }

    private static IServiceProvider ConfigureServices(AppSettings appSettings)
    {
      // Init service container
      var services = new ServiceCollection()
        .AddLogging(config => config
          .SetMinimumLevel(LogLevel.Trace)
          .AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true }))
        .AddMediaToolkit(@"C:\ffmpeg\ffmpeg.exe")
        .AddSingleton<IProgressIndicator, ProgressIndicatorConsole>()
        .AddSingleton<IShellAppContext>(_shellAppContext)
        .AddSingleton<ShellAppContext>(_shellAppContext)
        .AddSingleton(PhysicalConsole.Singleton)
        .AddSingleton<IFileSystem, FileSystem>()
        .AddSingleton<IClock, Clock>()
        .AddTransient<ITaskExecutionContext, TaskExecutionContext>();

      VideoModule.ConfigureServices(services);

      // DB connection
      var catalogName = appSettings.Catalogs.ContainsKey(appSettings.StartupCatalog)
        ? appSettings.StartupCatalog
        : appSettings.Catalogs.First().Key;

      var catalogSettings = appSettings.Catalogs[catalogName];
      services.AddSingleton<ICatalogSettings>(x => catalogSettings);

      // Modules
      AppEngineModule.ConfigureContainer(services, catalogSettings);
      VideoImprintModule.ConfigureContainer(services);

      var result = services.BuildServiceProvider();
      return result;
    }

    /// <summary>
    /// Creates the default app settings.
    /// </summary>
    private static AppSettings CreateDefaultSettings()
    {
      var appDataPath = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
      var mmDataPath = Path.Combine(appDataPath, ".mediaman");
      Directory.CreateDirectory(mmDataPath);
      var defaultDbPath = Path.Combine(mmDataPath, "default.db");

      var connectionString = new SqliteConnectionStringBuilder
      {
        DataSource = defaultDbPath
      }.ToString();


      var defaultCatalogSettings = new CatalogSettings
      {
        CatalogName = "default",
        ConnectionString = connectionString,
        MediaRoots = new Dictionary<string, string>()
      };
      var settings = new AppSettings
      {
        StartupCatalog = "default"
      };

      settings.Catalogs = new Dictionary<string, CatalogSettings>
      {
        { "default", defaultCatalogSettings }
      };

      return settings;
    }
  }
}
