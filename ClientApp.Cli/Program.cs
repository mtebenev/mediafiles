using System;
using System.IO.Abstractions;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
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

namespace Mt.MediaFiles.ClientApp.Cli
{
  [Command("mediaman")]
  [Subcommand(
    typeof(Shell),
    typeof(Commands.CommandCheckStatus),
    typeof(Commands.CommandScan),
    typeof(Commands.CommandUpdate),
    typeof(Commands.CommandSearchVideo))]
  internal class Program
  {
    public const int CommandExitResult = -1;
    public const int CommandResultContinue = 0;

    private static IServiceProvider _services;
    private static ShellAppContext _shellAppContext;

    public static async Task<int> Main(string[] args)
    {
      var result = 0;
      try
      {
        NLog.LogManager.LoadConfiguration("nlog.config");

        var environmentName = Environment.GetEnvironmentVariable("MM_ENVIRONMENT");

        var configuration = new ConfigurationBuilder()
          .SetBasePath(AppContext.BaseDirectory)
          .AddJsonFile("appsettings.json", true)
          .AddJsonFile($"appsettings.{environmentName}.json", true)
          .AddEnvironmentVariables()
          .Build();

        var appSettings = configuration.Get<AppSettings>();
        appSettings = DefaultSettings.FillDefaultSettings(
          appSettings,
          new EnvironmentWrapper(),
          new FileSystem()
        );

        _shellAppContext = new ShellAppContext(appSettings);

        // Open startup or first catalog
        if(appSettings.Catalogs.Count == 0)
          throw new InvalidOperationException("No catalogs defined");

        _services = ConfigureServices(appSettings);
        await _shellAppContext.OpenCatalog(_services);

        var app = new CommandLineApplication<Program>();
        app.Conventions
          .UseDefaultConventions()
          .UseConstructorInjection(_services);

        result = await app.ExecuteAsync(args);
      }
      finally
      {
        _shellAppContext?.Catalog.Dispose();
      }
      return result;
    }

    /// <summary>
    /// Invoked only if none command could be found (the default command).
    /// </summary>
    public Task<int> OnExecuteAsync(CommandLineApplication app)
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
        .AddMediaToolkit(@"C:\ProgramData\chocolatey\bin\ffmpeg.exe", null, FfLogLevel.Fatal)
        .AddSingleton<IProgressIndicator, ProgressIndicatorConsole>()
        .AddSingleton<IShellAppContext>(_shellAppContext)
        .AddSingleton(_shellAppContext)
        .AddSingleton(PhysicalConsole.Singleton)
        .AddSingleton<IFileSystem, FileSystem>()
        .AddSingleton<IClock, Clock>()
        .AddSingleton<IPathArgumentResolver, PathArgumentResolver>()
        .AddTransient<ITaskExecutionContext, TaskExecutionContext>();

      VideoModule.ConfigureServices(services);

      // Catalog settings
      var catalogSettings = appSettings.Catalogs[appSettings.StartupCatalog];
      services.AddSingleton<ICatalogSettings>(x => catalogSettings);

      // Modules
      AppEngineModule.ConfigureContainer(services, catalogSettings);
      VideoImprintModule.ConfigureContainer(services);

      var result = services.BuildServiceProvider();
      return result;
    }
  }
}
