using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Mt.MediaFiles.AppEngine.Cataloging;

namespace Mt.MediaFiles.ClientApp.Cli.Commands
{
  [Command("shell")]
  [Subcommand(
    typeof(ShellCommandCd),
    typeof(ShellCommandCls),
    typeof(ShellCommandExit),
    typeof(ShellCommandFindDuplicates),
    typeof(ShellCommandFindVideoDuplicates),
    typeof(ShellCommandGetInfo),
    typeof(ShellCommandLs),
    typeof(ShellCommandResetCatalog),
    typeof(ShellCommandScan),
    typeof(ShellCommandSearch),
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

    public async Task<int> OnExecuteAsync(IServiceProvider serviceProvider, ILogger<Shell> logger)
    {
      var commandResult = 0;

      // Do execution
      do
      {
        var prompt = CreatePrompt(_shellAppContext.CurrentItem);
        var commandInput = Prompt.GetString(prompt, promptColor: ConsoleColor.DarkGray);

        if(!string.IsNullOrEmpty(commandInput))
        {
          var commandArgs = commandInput.Split(' ');
          commandResult = await ExecuteShellCommandAsync(serviceProvider, commandArgs, logger);
        }
      } while(commandResult != Program.CommandExitResult);

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

    private async Task<int> ExecuteShellCommandAsync(IServiceProvider serviceProvider, string[] commandArgs, ILogger<Shell> logger)
    {
      var commandResult = Program.CommandResultContinue;

      try
      {
        // Note: avoid reusing shell application because this is not a scenario the fully supported by CommandLineUtils
        // Specifically the command options (profile option in scan) are not cleared between the calls.
        var shellApp = new CommandLineApplication<Shell>();
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
