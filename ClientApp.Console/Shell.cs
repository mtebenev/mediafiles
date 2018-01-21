using System;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Cataloging;

namespace Mt.MediaMan.ClientApp.Console
{
  internal class Shell
  {
    private readonly Catalog _catalog;

    public Shell()
    {
      _catalog = Catalog.CreateCatalog();
    }

    public void Run()
    {
      var isExit = false;

      while(!isExit)
      {
        var input = ReadLine.Read(">");
        if(input == "exit")
          isExit = true;
        else
        {
          Task.Run(async () =>
          {
            await ExecuteCommand(input);
          });
        }
      }

    }

    private async Task ExecuteCommand(string input)
    {
      if(input == "scan")
      {
        var commandScan = new ShellCommandScan();
        await commandScan.ExecuteAsync(_catalog);
      }
    }
  }
}
