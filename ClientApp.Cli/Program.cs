using System;
using System.Linq;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mt.MediaMan.AppEngine.Commands;
using Mt.MediaMan.ClientApp.Cli.Commands;
using Mt.MediaMan.ClientApp.Cli.Configuration;
using NLog.Extensions.Logging;

namespace Mt.MediaMan.ClientApp.Cli
{
  [Command("mediaman")]
  [Subcommand(
    typeof(Shell),
    typeof(CheckStatus),
    typeof(Update))]
  internal class Program
  {
    public const int CommandExitResult = -1;
    public const int CommandResultContinue = 0;

    private static IServiceProvider _services;
    private static ShellAppContext _shellAppContext;

    public static async Task<int> Main(string[] args)
    {
      Console.SetWindowSize(220, 54);
      NLog.LogManager.LoadConfiguration("nlog.config");

      var configuration = new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json")
        .Build();

      var appSettings = configuration.Get<AppSettings>();
      _shellAppContext = new ShellAppContext(appSettings);

      // Open startup or first catalog
      if(appSettings.Catalogs.Count == 0)
        throw new InvalidOperationException("No catalogs defined");

      var catalogName = appSettings.Catalogs.ContainsKey(appSettings.StartupCatalog)
        ? appSettings.StartupCatalog
        : appSettings.Catalogs.First().Key;

      await _shellAppContext.OpenCatalog(catalogName);

      // Init service container
      Program._services = new ServiceCollection()
        .AddLogging(config => config
          .SetMinimumLevel(LogLevel.Trace)
          .AddNLog(new NLogProviderOptions {CaptureMessageTemplates = true, CaptureMessageProperties = true}))
        .AddSingleton<IProgressIndicator, ProgressIndicatorConsole>()
        .AddSingleton(_shellAppContext)
        .AddSingleton(PhysicalConsole.Singleton)
        .AddTransient<ICommandExecutionContext, CommandExecutionContext>()
        .BuildServiceProvider();

      var app = new CommandLineApplication<Program>();
      app.Conventions
        .UseDefaultConventions()
        .UseConstructorInjection(Program._services);

      var result = await app.ExecuteAsync(args);
      return result;
    }

    /// <summary>
    /// Invoked only if none command could be found (the default command).
    /// </summary>
    public Task<int> OnExecuteAsync(CommandLineApplication app, IConsole console)
    {
      return app.ExecuteAsync(new[] { "shell" });
    }
  }
}
