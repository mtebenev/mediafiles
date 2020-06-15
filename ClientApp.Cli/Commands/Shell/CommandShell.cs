using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.ClientApp.Cli.Configuration;
using Mt.MediaFiles.ClientApp.Cli.Core;

namespace Mt.MediaFiles.ClientApp.Cli.Commands.Shell
{
  internal interface ICommandShell
  {
    /// <summary>
    /// The shell context.
    /// </summary>
    public IShellAppContext ShellAppContext { get; }
  }

  [Command("shell", Description = "Launches the app shell (the default command).")]
  [Subcommand(
    typeof(CommandShellCd),
    typeof(CommandShellCls),
    typeof(CommandShellExit),
    typeof(CommandShellSearchDuplicates),
    typeof(CommandShellGetInfo),
    typeof(CommandShellLs),
    typeof(CommandShellScan),
    typeof(CommandShellSearch),
    typeof(CommandShellSearchFiles),
    typeof(CommandShellSearchVideoDuplicates),
    typeof(CommandShellUpdate)
    )]
  [ExperimentalCommand]
  internal class CommandShell : AppCommandBase, ICommandShell
  {
    private readonly IAppSettingsManager _appSettingsManager;

    /// <summary>
    /// Ctor.
    /// </summary>
    public CommandShell(IAppSettingsManager appSettingsManager)
    {
      this._appSettingsManager = appSettingsManager;
    }

    /// <summary>
    /// The parameterless constructor used for child commands execution.
    /// </summary>
    public CommandShell()
    {
    }

    /// <summary>
    /// ICommandShell.
    /// </summary>
    public IShellAppContext ShellAppContext { get; set; }

    public async Task<int> OnExecuteAsync(IServiceProvider serviceProvider, ILogger<CommandShell> logger)
    {
      if(this._appSettingsManager == null)
      {
        throw new InvalidOperationException("The shell command should be instantiated with app settings manager.");
      }

      var commandResult = 0;
      var catalog = await this.OpenCatalogAsync();
      var catalogSettings = this.GetCatalogSetings();
      var shellAppContext = new ShellAppContext(this._appSettingsManager, catalog, catalogSettings);

      // Do execution
      do
      {
        var prompt = CreatePrompt(shellAppContext.CurrentItem);
        var commandInput = Prompt.GetString(prompt, promptColor: ConsoleColor.DarkGray);

        if (!string.IsNullOrEmpty(commandInput))
        {
          var commandArgs = commandInput.Split(' ');
          commandResult = await ExecuteShellCommandAsync(shellAppContext, serviceProvider, commandArgs, logger);
        }
      } while (commandResult != Program.CommandExitResult);

      return commandResult;
    }


    /// <summary>
    /// Creates string prompt for CLI
    /// </summary>
    private static string CreatePrompt(ICatalogItem currentItem)
    {
      var result = $"{currentItem.Path}>";
      return result;
    }

    private async Task<int> ExecuteShellCommandAsync(IShellAppContext shellAppContext, IServiceProvider serviceProvider, string[] commandArgs, ILogger<CommandShell> logger)
    {
      var commandResult = Program.CommandResultContinue;

      try
      {
        // Note: avoid reusing shell application because this is not a scenario the fully supported by CommandLineUtils
        // Specifically the command options (profile option in scan) are not cleared between the calls.
        var shellApp = new CommandLineApplication<CommandShell>();
        shellApp.Model.ShellAppContext = shellAppContext; // Setting shell app context explicitly because this is the newly created instance.
        shellApp.Conventions
          .UseDefaultConventions()
          .UseConstructorInjection(serviceProvider);

        commandResult = await shellApp.ExecuteAsync(commandArgs);
      }
      catch(Exception e)
      {
        shellAppContext.Reporter.Error(e.Message);
        logger.LogError(e, "Error occurred during shell command execution:");
      }

      return commandResult;
    }
  }
}
