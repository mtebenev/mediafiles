using System;
using System.Linq;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Commands;
using Mt.MediaMan.AppEngine.Scanning;

namespace Mt.MediaMan.ClientApp.Cli
{
  /// <summary>
  /// Scans new files to catalog
  /// </summary>
  [Command("get-info", Description = "Prints full information on a catalog item")]
  internal class ShellCommandGetInfo : ShellCommandBase
  {
    private readonly ICommandExecutionContext _executionContext;
    private readonly ShellContext _shellContext;

    public ShellCommandGetInfo(ICommandExecutionContext executionContext, ShellContext shellContext)
    {
      _executionContext = executionContext;
      _shellContext = shellContext;
    }

    [Argument(0, "itemNameOrId")]
    public string ItemNameOrId { get; set; }

    protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
    {
      var item = await GetItemByNameOrIdAsync(_shellContext, _executionContext, ItemNameOrId);

      if(item == null)
        throw new ArgumentException("Cannot load catalog item", nameof(ItemNameOrId));

      var infoPart = await item.GetInfoPartAsync<InfoPartVideo>();
      Console.WriteLine($"Width: {infoPart.VideoWidth}, Height: {infoPart.VideoHeight}");

      return 0;
    }
  }
}
