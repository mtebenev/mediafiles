using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.ClientApp.Cli.Core;

namespace Mt.MediaFiles.ClientApp.Cli.Commands.Shell
{
  [Command("shell", Description = "Launches the app shell (the default command")]
  [Subcommand(
    typeof(CommandShellCd),
    typeof(CommandShellCls),
    typeof(CommandShellExit),
    typeof(CommandShellSearchDuplicates),
    typeof(CommandShellGetInfo),
    typeof(CommandShellLs),
    typeof(CommandShellResetCatalog),
    typeof(CommandShellScan),
    typeof(CommandShellSearch),
    typeof(CommandShellSearchFiles),
    typeof(CommandShellSearchVideoDuplicates),
    typeof(CommandShellUpdate)
    )]
  [ExperimentalCommand]
  internal class CommandShell
  {
    private readonly IShellAppContext _shellAppContext;

    public CommandShell(IShellAppContext shellAppContext)
    {
      _shellAppContext = shellAppContext;
    }

    public async Task<int> OnExecuteAsync(IServiceProvider serviceProvider, ILogger<CommandShell> logger)
    {
      var commandResult = 0;

      // Do execution
      do
      {
        var prompt = CreatePrompt(_shellAppContext.CurrentItem);
        var commandInput = Prompt.GetString(prompt, promptColor: ConsoleColor.DarkGray);

        if (!string.IsNullOrEmpty(commandInput))
        {
          var commandArgs = commandInput.Split(' ');
          commandResult = await ExecuteShellCommandAsync(serviceProvider, commandArgs, logger);
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

    private async Task<int> ExecuteShellCommandAsync(IServiceProvider serviceProvider, string[] commandArgs, ILogger<CommandShell> logger)
    {
      var commandResult = Program.CommandResultContinue;

      try
      {
        // Note: avoid reusing shell application because this is not a scenario the fully supported by CommandLineUtils
        // Specifically the command options (profile option in scan) are not cleared between the calls.
        var shellApp = new CommandLineApplication<CommandShell>();
        shellApp.Conventions
          .UseDefaultConventions()
          .UseConstructorInjection(serviceProvider);

        commandResult = await shellApp.ExecuteAsync(commandArgs);
      }
      catch(Exception e)
      {
        logger.LogError(e, "Error occurred during shell command execution:");
      }

      return commandResult;
    }
  }
}
