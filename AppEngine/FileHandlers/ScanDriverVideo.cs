using System.IO.Abstractions;
using System.Linq;
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
      var fileSystem = new FileSystem();
      FfprobeType ffProbeOutput;
      using(var engine = new Engine(@"C:\ffmpeg\FFmpeg.exe", fileSystem))
      {
        ffProbeOutput = await engine.GetMetadataAsync(filePath);
      }

      infoPartVideo.Duration = ffProbeOutput.Format.Duration;
      FillVideoStreamInfo(ffProbeOutput, infoPartVideo);

      catalogItemData.Apply(infoPartVideo);
    }

    /// <summary>
    /// Finds first video stream and extracts information for that
    /// </summary>
    private void FillVideoStreamInfo(FfprobeType ffProbeOutput, InfoPartVideo infoPartVideo)
    {
      var videoStream = ffProbeOutput.Streams.FirstOrDefault(s => s.Codec_Type == "video");
      if(videoStream != null)
      {
        infoPartVideo.VideoHeight = videoStream.Height;
        infoPartVideo.VideoWidth = videoStream.Width;
        infoPartVideo.VideoCodecName = videoStream.Codec_Name;
        infoPartVideo.VideoCodecLongName = videoStream.Codec_Long_Name;
      }
    }
  }
}

