using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace Mt.MediaMan.ClientApp.Cli
{
  /// <summary>
  /// Scans new files to catalog
  /// </summary>
  [Command("ls", Description = "Prints content of the current folder")]
  internal class ShellCommandLs : ShellCommandBase
  {
    private readonly ShellContext _shellContext;

    public ShellCommandLs(ShellContext shellContext)
    {
      _shellContext = shellContext;
    }

    protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
    {
      var currentItem = _shellContext.CurrentItem;
      var children = await currentItem.GetChildrenAsync();

      TableBuilder tb = new TableBuilder();
      tb.AddRow("ID", "Name", "Size");
      tb.AddRow("--", "----", "----");

      foreach(var catalogItem in children)
        tb.AddRow(catalogItem.CatalogItemId, catalogItem.Name, catalogItem.Size);


      Console.Write(tb.Output());
      return 0;
    }
  }
}
