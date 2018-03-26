using System;
using System.Linq;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace Mt.MediaMan.ClientApp.Cli
{
  /// <summary>
  /// Scans new files to catalog
  /// </summary>
  [Command(Description = "Prints content of the current folder")]
  internal class ShellCommandLs : ShellCommandBase
  {
    /// <summary>
    /// Injected
    /// </summary>
    public Shell Parent { get; set; }

    protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
    {
      await Parent.InitializeAsync();

      var currentItem = Parent.CurrentItem;
      var children = await currentItem.GetChildrenAsync();

      var printData = children
        .Select(c => c.Name)
        .ToList();
      foreach(var name in printData)
      {
        Console.WriteLine(name);
      }

      return 0;
    }
  }
}
