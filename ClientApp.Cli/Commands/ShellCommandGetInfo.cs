using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.Scanning;

namespace Mt.MediaFiles.ClientApp.Cli.Commands
{
  /// <summary>
  /// Retrieves item info by name or id
  /// </summary>
  [Command("get-info", Description = "Prints full information on a catalog item")]
  internal class ShellCommandGetInfo : ShellCommandBase
  {
    private readonly IShellAppContext _shellAppContext;

    public ShellCommandGetInfo(IShellAppContext shellAppContext)
    {
      this._shellAppContext = shellAppContext;
    }

    [Argument(0, "itemNameOrId")]
    public string ItemNameOrId { get; set; }

    public async Task<int> OnExecuteAsync()
    {
      var item = await this.GetItemByNameOrIdAsync(
        this._shellAppContext,
        this.ItemNameOrId);

      if(item == null)
        throw new ArgumentException("Cannot load catalog item", nameof(ItemNameOrId));

      var infoParts = await item.GetInfoPartNamesAsync();
      foreach(var partName in infoParts)
      {
        if(partName == nameof(InfoPartVideo))
          await PrintVideoPartAsync(item);
        else if(partName == nameof(InfoPartBook))
          await PrintBookPartAsync(item);
        else
          throw new InvalidOperationException($"Unknown info part: {partName}");
      }

      return Program.CommandResultContinue;
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
