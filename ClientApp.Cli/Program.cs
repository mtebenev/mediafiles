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
using Mt.MediaFiles.AppEngine;
using Mt.MediaFiles.AppEngine.Video.VideoImprint;
using MediaToolkit.Options;
using Mt.MediaFiles.ClientApp.Cli.Core;
using McMaster.Extensions.CommandLineUtils.Conventions;
using Mt.MediaFiles.AppEngine.Video.Thumbnail;
using Mt.MediaFiles.AppEngine.Cataloging;

namespace Mt.MediaFiles.ClientApp.Cli
{
  internal class Program
  {
    public const int CommandExitResult = -1;
    public const int CommandResultContinue = 0;

    public static async Task<int> Main(string[] args)
    {
      var result = 0;
      ILogger logger = null;
      try
      {
        NLog.LogManager.LoadConfiguration("nlog.config");

        var environmentWrapper = new EnvironmentWrapper();
        var fileSystem = new FileSystem();
        var appSettingsManager = AppSettingsManager.Create(environmentWrapper, fileSystem);

        // Open startup or first catalog
        if(appSettingsManager.AppSettings.Catalogs.Count == 0)
          throw new InvalidOperationException("No catalogs defined. Please check and fix app configuration.");

        using(var services = ConfigureServices(appSettingsManager))
        {
          logger = services.GetRequiredService<ILoggerFactory>().CreateLogger<Program>();

          var app = CreateCommandLineApplication(services, appSettingsManager.AppSettings.ExperimentalMode);
          result = await app.ExecuteAsync(args);
        }
      }
      catch(Exception e)
      {
        var reporter = new ConsoleReporter(PhysicalConsole.Singleton);
        reporter.Error(e.Message);
        if(logger != null)
        {
          logger.LogError(e, "An error occurred during the command execution.");
        }
      }
      return result;
    }

    private static ServiceProvider ConfigureServices(AppSettingsManager appSettingsManager/*, ICatalogSettings catalogSettings*/)
    {
      // Init service container
      var services = new ServiceCollection()
        .AddLogging(config => config
          .SetMinimumLevel(LogLevel.Trace)
          .AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true }))
        .AddMediaToolkit(@"C:\ProgramData\chocolatey\bin\ffmpeg.exe", null, FfLogLevel.Fatal)
        .AddSingleton<AppSettings>(appSettingsManager.AppSettings)
        .AddSingleton<AppEngineSettings>(appSettingsManager.AppEngineSettings)
        .AddSingleton<IAppSettingsManager>(appSettingsManager)
        .AddTransient<ICatalogFactory, CatalogFactory>()
        .AddSingleton(PhysicalConsole.Singleton)
        .AddSingleton<IReporter, ConsoleReporter>()
        .AddSingleton<IFileSystem, FileSystem>()
        .AddSingleton<IEnvironment, EnvironmentWrapper>()
        .AddSingleton<IClock, Clock>()
        .AddSingleton<IPathArgumentResolver, PathArgumentResolver>()
        .AddTransient<ITaskExecutionContext, TaskExecutionContext>()
        .AddSingleton<DbConnectionProvider>()
        .AddSingleton<IDbConnectionProvider>(c => c.GetRequiredService<DbConnectionProvider>())
        .AddSingleton<IDbConnectionSource>(c => c.GetRequiredService<DbConnectionProvider>());

      VideoModule.ConfigureServices(services);

      // Modules
      AppEngineModule.ConfigureContainer(services);
      VideoImprintModule.ConfigureContainer(services);
      ThumbnailModule.ConfigureContainer(services);

      var result = services.BuildServiceProvider();
      return result;
    }

    /// <summary>
    /// Creates and configures the command-line application object.
    /// </summary>
    private static CommandLineApplication<Program> CreateCommandLineApplication(IServiceProvider serviceProvider, bool isExperimentalMode)
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
        .UseConstructorInjection(serviceProvider);
      return app;
    }
  }
}
