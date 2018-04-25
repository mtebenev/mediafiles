using System;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.CatalogStorage;
using Mt.MediaMan.AppEngine.Scanning;

namespace Mt.MediaMan.AppEngine.FileHandlers
{
  /// <summary>
  /// Extracts video information
  /// </summary>
  internal class ScanDriverVideo : IScanDriver
  {
    public Task ScanAsync(IScanContext scanContext, int catalogItemId, FileStoreEntryContext fileStoreEntryContext, CatalogItemData catalogItemData)
    {
      var infoPartVideo = catalogItemData.GetOrCreate<InfoPartVideo>();

      infoPartVideo.VideoWidth = 300;
      infoPartVideo.VideoHeight = new Random().Next();

      catalogItemData.Apply(infoPartVideo);

      return Task.CompletedTask;
    }
  }
}
