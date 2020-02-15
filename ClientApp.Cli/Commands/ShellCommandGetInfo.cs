using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Commands;
using Mt.MediaMan.AppEngine.Scanning;

namespace Mt.MediaMan.ClientApp.Cli.Commands
{
  /// <summary>
  /// Retrieves item info by name or id
  /// </summary>
  [Command("get-info", Description = "Prints full information on a catalog item")]
  internal class ShellCommandGetInfo : ShellCommandBase
  {
    private readonly ICommandExecutionContext _executionContext;
    private readonly ShellAppContext _shellAppContext;

    public ShellCommandGetInfo(ICommandExecutionContext executionContext, ShellAppContext shellAppContext)
    {
      _executionContext = executionContext;
      _shellAppContext = shellAppContext;
    }

    [Argument(0, "itemNameOrId")]
    public string ItemNameOrId { get; set; }

    protected override async Task<int> OnExecuteAsync(CommandLineApplication app)
    {
      var item = await GetItemByNameOrIdAsync(_shellAppContext, _executionContext, ItemNameOrId);

      if(item == null)
        throw new ArgumentException("Cannot load catalog item", nameof(ItemNameOrId));

      var infoParts = await item.GetInfoPartNamesAsync();
      foreach(string partName in infoParts)
      {
        if(partName == nameof(InfoPartVideo))
          await PrintVideoPartAsync(item);
        else if(partName == nameof(InfoPartBook))
          await PrintBookPartAsync(item);
        else
          throw new InvalidOperationException($"Unknown info part: {partName}");
      }

      return 0;
    }

    private async Task PrintBookPartAsync(ICatalogItem catalogItem)
    {
      var infoPart = await catalogItem.GetInfoPartAsync<InfoPartBook>();
      _shellAppContext.Console.WriteLine("Book");
      _shellAppContext.Console.WriteLine($"Title: {infoPart.Title}");
      _shellAppContext.Console.WriteLine($"Authors: {infoPart.GetAuthorsString()}");
    }

    private async Task PrintVideoPartAsync(ICatalogItem catalogItem)
    {
      var infoPart = await catalogItem.GetInfoPartAsync<InfoPartVideo>();
      _shellAppContext.Console.WriteLine("Video");
      _shellAppContext.Console.WriteLine($"Title: {infoPart.Title}");
      _shellAppContext.Console.WriteLine($"Duration: {TimeSpan.FromSeconds(infoPart.Duration)}");
      _shellAppContext.Console.WriteLine($"Width: {infoPart.VideoWidth}");
      _shellAppContext.Console.WriteLine($"Height: {infoPart.VideoHeight}");
      _shellAppContext.Console.WriteLine($"Codec: {infoPart.VideoCodecLongName}");
    }
  }
}
