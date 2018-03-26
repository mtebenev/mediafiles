using System;
using System.Linq;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace Mt.MediaMan.ClientApp.Cli
{
  /// <summary>
  /// Scans new files to catalog
  /// </summary>
  [Command(Description = "Changes current directory")]
  internal class ShellCommandCd : ShellCommandBase
  {
    /// <summary>
    /// Injected
    /// </summary>
    public Shell Parent { get; set; }

    [Argument(0, "itemName")]
    public string ItemName { get; set; }

    protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
    {
      var currentItem = Parent.CurrentItem;
      var children = await currentItem.GetChildrenAsync();

      var child = children
        .FirstOrDefault(c => c.Name.Equals(ItemName, StringComparison.InvariantCultureIgnoreCase));

      if(child != null)
        Parent.CurrentItem = child;

      return 0;
    }
  }
}
