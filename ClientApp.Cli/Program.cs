using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Commands;
using Mt.MediaMan.AppEngine.Ebooks;
using NLog.Extensions.Logging;

namespace Mt.MediaMan.ClientApp.Cli
{
  internal class Program
  {
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

      var catalog = await OpenCatalog(appSettings);
      _shellAppContext = new ShellAppContext(catalog.RootItem);
      
      // Init service container
      _services = new ServiceCollection()
        .AddLogging(config => config
          .SetMinimumLevel(LogLevel.Trace)
          .AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true }))
        .AddSingleton(catalog)
        .AddSingleton<IProgressIndicator, ProgressIndicatorConsole>()
        .AddSingleton<ICommandExecutionContext, CommandExecutionContext>()
        .AddSingleton(PhysicalConsole.Singleton)
        .AddSingleton(_shellAppContext)
        .BuildServiceProvider();

      int commandResult = 0;

      // Do execution
      do
      {
        var prompt = CreatePrompt(_shellAppContext.CurrentItem);
        var commandInput = Prompt.GetString(prompt, promptColor: ConsoleColor.DarkBlue);

        if(!String.IsNullOrEmpty(commandInput))
        {
          var commandArgs = commandInput.Split(' ');

          // Create Shell app
          var shellApp = new CommandLineApplication<Shell>();
          shellApp.Conventions
            .UseDefaultConventions()
            .UseConstructorInjection(_services);

          commandResult = ExecuteShellCommand(shellApp, commandArgs);
        }
      } while(commandResult != CommandExitResult);

      return 1;
    }


    public const int CommandExitResult = -1;

    /// <summary>
    /// Creates string prompt for CLI
    /// </summary>
    private static string CreatePrompt(ICatalogItem currentItem)
    {
      var result = $"{currentItem.Name}>";
      return result;
    }

    private static async Task<Catalog> OpenCatalog(AppSettings appSettings)
    {
      var storageConfiguration = new StorageConfiguration();
      EbooksModule.CreateStorageConfiguration(storageConfiguration);

      // Open catalog
      var catalog = Catalog.CreateCatalog(appSettings.ConnectionString);
      await catalog.OpenAsync(storageConfiguration);

      return catalog;
    }

    private static int ExecuteShellCommand(CommandLineApplication<Shell> shellApp, string[] commandArgs)
    {
      int commandResult = CommandExitResult;

      try
      {
        commandResult = shellApp.Execute(commandArgs);
        return commandResult;
      }
      catch (Exception e)
      {
        var logger = _services.GetService<ILogger<Program>>();
        logger.LogError(e, "Error occurred during shell command execution:");
      }

      return commandResult;
    }
  }
}
