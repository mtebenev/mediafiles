using System;
using System.Linq;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.CatalogStorage;
using OrchardCore.FileStorage;

namespace Mt.MediaMan.AppEngine.Scanning
{
  /// <summary>
  /// Extracts video information
  /// </summary>
  internal class ScanDriverVideo : IScanDriver
  {
    public Task<bool> IsSupportedAsync(IFileStoreEntry fileStoreEntry)
    {
      var supportedExtensions = new[] {".mkv", ".mp4", ".avi"};
      var result = supportedExtensions.Any(e => fileStoreEntry.Name.EndsWith(e, StringComparison.InvariantCultureIgnoreCase));

      return Task.FromResult(result);
    }

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
