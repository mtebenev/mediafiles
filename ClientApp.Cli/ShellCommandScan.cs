using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Commands;

namespace Mt.MediaMan.ClientApp.Console
{
  /// <summary>
  /// Scans new files to catalog
  /// </summary>
  internal class ShellCommandScan
  {
    public async Task ExecuteAsync(Catalog catalog)
    {
      var scanPath = @"C:\_films";
      var command = new CommandScanFiles();
      await command.Execute(catalog, scanPath);
    }
  }
}
