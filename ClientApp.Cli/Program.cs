using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Commands;
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
      _shellAppContext = new ShellAppContext(appSettings);
      await _shellAppContext.OpenCatalog();

      // Init service container
      _services = new ServiceCollection()
        .AddLogging(config => config
          .SetMinimumLevel(LogLevel.Trace)
          .AddNLog(new NLogProviderOptions {CaptureMessageTemplates = true, CaptureMessageProperties = true}))
        .AddSingleton<IProgressIndicator, ProgressIndicatorConsole>()
        .AddSingleton(_shellAppContext)
        .AddSingleton(PhysicalConsole.Singleton)
        .AddTransient<ICommandExecutionContext, CommandExecutionContext>()
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
    public const int CommandResultContinue = 0;

    /// <summary>
    /// Creates string prompt for CLI
    /// </summary>
    private static string CreatePrompt(ICatalogItem currentItem)
    {
      var result = $"{currentItem.Name}>";
      return result;
    }

    private static int ExecuteShellCommand(CommandLineApplication<Shell> shellApp, string[] commandArgs)
    {
      int commandResult = CommandExitResult;

      try
      {
        commandResult = shellApp.Execute(commandArgs);
        return commandResult;
      }
      catch(Exception e)
      {
        var logger = _services.GetService<ILogger<Program>>();
        logger.LogError(e, "Error occurred during shell command execution:");
      }

      return commandResult;
    }
  }
}
