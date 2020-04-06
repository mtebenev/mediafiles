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
using Mt.MediaFiles.ClientApp.Cli.Configuration;

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
