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

    public Task ScanAsync(IScanContext scanContext, int catalogItemId, IFileStoreEntry fileStoreEntry, IItemStorage itemStorage)
    {
      var infoPartVideo = new InfoPartVideo
      {
        CatalogItemId = catalogItemId,
        VideoWidth = 300,
        VideoHeight = new Random().Next()
      };

      return itemStorage.SaveInfoPartAsync(catalogItemId, infoPartVideo);
    }
  }
}
