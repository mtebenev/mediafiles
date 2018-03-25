using System;
using McMaster.Extensions.CommandLineUtils;

namespace Mt.MediaMan.ClientApp.Cli
{
  class Program
  {
    public static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);
    public static int CommandExitResult = -1;

    public int OnExecute()
    {
      int commandResult = 0;
      do
      {
        var commandInput = Prompt.GetString(">", promptColor: ConsoleColor.DarkBlue);

        if(!String.IsNullOrEmpty(commandInput))
        {
          var commandArgs = commandInput.Split(' ');
          commandResult = CommandLineApplication.Execute<Shell>(commandArgs);
        }

      } while(commandResult != CommandExitResult);

      return 1;
    }
  }
}
