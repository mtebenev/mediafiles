using System;
using System.IO.Abstractions;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using MediaToolkit;
using Mt.MediaFiles.ClientApp.Cli.Configuration;
using Mt.MediaFiles.AppEngine.Video;
using Mt.MediaFiles.AppEngine.Common;
using Mt.MediaFiles.AppEngine.Tasks;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using Mt.MediaFiles.ClientApp.Cli.Commands;
using Mt.MediaFiles.AppEngine;
using Mt.MediaFiles.AppEngine.Video.VideoImprint;
using Mt.MediaFiles.ClientApp.Cli.Ui;
using MediaToolkit.Options;
using Mt.MediaFiles.ClientApp.Cli.Core;
using McMaster.Extensions.CommandLineUtils.Conventions;
using System.Data;
using Microsoft.Data.Sqlite;
using Mt.MediaFiles.AppEngine.Video.Thumbnail;

namespace Mt.MediaFiles.ClientApp.Cli
{
  [Command("mediafiles")]
  [Subcommand(
    typeof(Shell),
    typeof(Commands.CommandCheckStatus),
    typeof(Commands.CommandResetCatalog),
    typeof(Commands.CommandScan),
    typeof(Commands.CommandSearchVideo),
    typeof(Commands.CommandSearchVideoDuplicates),
    typeof(Commands.CommandUpdate),
    typeof(Commands.Catalog.CommandCatalog))]
  internal class Program
  {
    public const int CommandExitResult = -1;
    public const int CommandResultContinue = 0;

    private static IServiceProvider _services;
    private static ShellAppContext _shellAppContext;

    public static async Task<int> Main(string[] args)
    {
      var result = 0;
      var isCatalogOpen = false;
      try
      {
        NLog.LogManager.LoadConfiguration("nlog.config");

        var environmentWrapper = new EnvironmentWrapper();
        var fileSystem = new FileSystem();
        var appSettingsManager = AppSettingsManager.Create(environmentWrapper, fileSystem);

        _shellAppContext = new ShellAppContext(appSettingsManager);

        // Open startup or first catalog
        if(appSettingsManager.AppSettings.Catalogs.Count == 0)
          throw new InvalidOperationException("No catalogs defined. Please check and fix app configuration.");

        var catalogSettings = appSettingsManager.AppSettings.Catalogs[appSettingsManager.AppSettings.StartupCatalog];
        var dbConnection = OpenDbConnection(catalogSettings);

        _services = ConfigureServices(appSettingsManager, catalogSettings, dbConnection);
        await _shellAppContext.OpenCatalog(_services);
        isCatalogOpen = true;

        var app = CreateCommandLineApplication(appSettingsManager.AppSettings.ExperimentalMode);
        result = await app.ExecuteAsync(args);
      }
      catch(Exception e)
      {
        Console.WriteLine("An error occurred.");
        Console.WriteLine(e.ToString());
      }
      finally
      {
        if(isCatalogOpen)
        {
          _shellAppContext?.Catalog?.Dispose();
        }
      }
      return result;
    }

    /// <summary>
    /// Invoked only if none command could be found (the default command).
    /// </summary>
    public Task<int> OnExecuteAsync(CommandLineApplication app, AppSettings appSettings, IConsole console)
    {
      Task<int> result;
      if(appSettings.ExperimentalMode)
        result = app.ExecuteAsync(new[] { "shell" });
      else
      {
        console.WriteLine("Please specify a command.");
        app.ShowHelp();
        return Task.FromResult(0);
      }

      return result;
    }

    private static IServiceProvider ConfigureServices(AppSettingsManager appSettingsManager, ICatalogSettings catalogSettings, IDbConnection dbConnection)
    {
      // Init service container
      var services = new ServiceCollection()
        .AddLogging(config => config
          .SetMinimumLevel(LogLevel.Trace)
          .AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true }))
        .AddMediaToolkit(@"C:\ProgramData\chocolatey\bin\ffmpeg.exe", null, FfLogLevel.Fatal)
        .AddSingleton<AppSettings>(appSettingsManager.AppSettings)
        .AddSingleton<AppEngineSettings>(appSettingsManager.AppEngineSettings)
        .AddSingleton<ICatalogSettings>(catalogSettings)
        .AddSingleton<IProgressIndicator, ProgressIndicatorConsole>()
        .AddSingleton<IShellAppContext>(_shellAppContext)
        .AddSingleton(_shellAppContext)
        .AddSingleton(PhysicalConsole.Singleton)
        .AddSingleton<IFileSystem, FileSystem>()
        .AddSingleton<IClock, Clock>()
        .AddSingleton<IPathArgumentResolver, PathArgumentResolver>()
        .AddTransient<ITaskExecutionContext, TaskExecutionContext>();

      VideoModule.ConfigureServices(services);

      // Modules
      AppEngineModule.ConfigureContainer(services, catalogSettings, dbConnection);
      VideoImprintModule.ConfigureContainer(services);
      ThumbnailModule.ConfigureContainer(services);

      var result = services.BuildServiceProvider();
      return result;
    }

    /// <summary>
    /// Creates and configures the command-line application object.
    /// </summary>
    private static CommandLineApplication<Program> CreateCommandLineApplication(bool isExperimentalMode)
    {
      var app = new CommandLineApplication<Program>();
      app.Conventions
        .AddConvention(new AttributeConvention())
        .UseCommandAttribute()
        .UseVersionOptionFromMemberAttribute()
        .UseVersionOptionAttribute()
        .UseHelpOptionAttribute()
        .UseOptionAttributes()
        .UseArgumentAttributes()
        .AddConvention(new SubcommandAttributeConventionEx(isExperimentalMode))
        .SetAppNameFromEntryAssembly()
        .SetRemainingArgsPropertyOnModel()
        .SetSubcommandPropertyOnModel()
        .SetParentPropertyOnModel()
        .UseOnExecuteMethodFromModel()
        .UseOnValidateMethodFromModel()
        .UseOnValidationErrorMethodFromModel()
        .UseDefaultHelpOption()
        .UseCommandNameFromModelType()
        .UseConstructorInjection(_services);
      return app;
    }

    /// <summary>
    /// Initializes connection.
    /// </summary>
    private static IDbConnection OpenDbConnection(ICatalogSettings catalogSettings)
    {
      IDbConnection result = null;
      try
      {
        result = new SqliteConnection(catalogSettings.ConnectionString);
        result.Open();
      }
      catch(Exception e)
      {
        throw new InvalidOperationException(
          $"Could not open the sqlite database \"{catalogSettings.ConnectionString}\". Please make sure that the specified directory exists",
          e
        );
      }

      return result;
    }
  }
}
