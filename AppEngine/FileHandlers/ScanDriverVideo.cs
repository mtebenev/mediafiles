using System.Threading.Tasks;
using MediaToolkit;
using MediaToolkit.Model;
using Mt.MediaMan.AppEngine.CatalogStorage;
using Mt.MediaMan.AppEngine.Scanning;

namespace Mt.MediaMan.AppEngine.FileHandlers
{
  /// <summary>
  /// Extracts video information
  /// </summary>
  internal class ScanDriverVideo : IScanDriver
  {
    public async Task ScanAsync(IScanContext scanContext, int catalogItemId, FileStoreEntryContext fileStoreEntryContext, CatalogItemData catalogItemData)
    {
      var infoPartVideo = catalogItemData.GetOrCreate<InfoPartVideo>();

      var filePath = await fileStoreEntryContext.AccessFilePathAsync();
      var inputFile = new MediaFile(filePath);
      using(var engine = new Engine(@"C:\ffmpeg\FFmpeg.exe"))
      {
        engine.GetMetadata(inputFile);
      }

      var tokens = inputFile.Metadata.VideoData.FrameSize.Split('x');

      infoPartVideo.VideoWidth = int.Parse(tokens[0]);
      infoPartVideo.VideoHeight = int.Parse(tokens[1]);

      catalogItemData.Apply(infoPartVideo);
    }
  }
}
