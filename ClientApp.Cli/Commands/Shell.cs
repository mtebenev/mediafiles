using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Mt.MediaMan.AppEngine.Cataloging;

namespace Mt.MediaMan.ClientApp.Cli.Commands
{
  [Command("shell")]
  [Subcommand(
    typeof(ShellCommandCd),
    typeof(ShellCommandCls),
    typeof(ShellCommandCreateBook),
    typeof(ShellCommandExit),
    typeof(ShellCommandFindDuplicates),
    typeof(ShellCommandFindVideoDuplicates),
    typeof(ShellCommandGetInfo),
    typeof(ShellCommandLs),
    typeof(ShellCommandResetCatalog),
    typeof(ShellCommandScan),
    typeof(ShellCommandSearch),
    typeof(ShellCommandSearchBook),
    typeof(ShellCommandSearchFiles),
    typeof(ShellCommandUpdate)
    )]
  internal class Shell
  {
    private readonly IShellAppContext _shellAppContext;

    public Shell(IShellAppContext shellAppContext)
    {
      _shellAppContext = shellAppContext;
    }

    public async Task<int> OnExecuteAsync(CommandLineApplication app, ILogger<Shell> logger)
    {
      int commandResult = 0;

      // Do execution
      do
      {
        var prompt = CreatePrompt(_shellAppContext.CurrentItem);
        var commandInput = Prompt.GetString(prompt, promptColor: ConsoleColor.DarkBlue);

        if(!String.IsNullOrEmpty(commandInput))
        {
          var commandArgs = commandInput.Split(' ');
          commandResult = await ExecuteShellCommandAsync(app, commandArgs, logger);
        }
      } while(commandResult != Program.CommandExitResult);

      return commandResult;
    }


    /// <summary>
    /// Creates string prompt for CLI
    /// </summary>
    private static string CreatePrompt(ICatalogItem currentItem)
    {
      var result = $"{currentItem.Name}>";
      return result;
    }

    private async Task<int> ExecuteShellCommandAsync(CommandLineApplication app, string[] commandArgs, ILogger<Shell> logger)
    {
      int commandResult = Program.CommandResultContinue;

      try
      {
        commandResult = await app.ExecuteAsync(commandArgs);
      }
      catch(Exception e)
      {
        logger.LogError(e, "Error occurred during shell command execution:");
      }

      return commandResult;
    }
  }
}
