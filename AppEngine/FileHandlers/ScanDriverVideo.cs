using System.Linq;
using System.Threading.Tasks;
using MediaToolkit.Model;
using MediaToolkit.Services;
using MediaToolkit.Tasks;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using Mt.MediaFiles.AppEngine.Scanning;

namespace Mt.MediaFiles.AppEngine.FileHandlers
{
  /// <summary>
  /// Extracts video information
  /// </summary>
  internal class ScanDriverVideo : IScanDriver
  {
    private readonly IMediaToolkitService _mediaToolkitService;

    public ScanDriverVideo(IMediaToolkitService mediaToolkitService)
    {
      this._mediaToolkitService = mediaToolkitService;
    }

    /// <summary>
    /// IScanDriver.
    /// </summary>
    public async Task ScanAsync(int catalogItemId, FileStoreEntryContext fileStoreEntryContext, CatalogItemData catalogItemData)
    {
      var infoPartVideo = catalogItemData.GetOrCreate<InfoPartVideo>();

      var filePath = await fileStoreEntryContext.AccessFilePathAsync();
      var metadataTask = new FfTaskGetMetadata(filePath);
      var taskResult = await this._mediaToolkitService.ExecuteAsync(metadataTask);

      infoPartVideo.Duration = (int)taskResult.Metadata.Format.Duration.TotalMilliseconds;
      this.FillVideoStreamInfo(taskResult.Metadata, infoPartVideo);

      catalogItemData.Apply(infoPartVideo);
    }

    /// <summary>
    /// Finds first video stream and extracts information for that
    /// </summary>
    private void FillVideoStreamInfo(FfProbeOutput ffProbeOutput, InfoPartVideo infoPartVideo)
    {
      // Stream-specific properties
      var videoStream = ffProbeOutput.Streams.FirstOrDefault(s => s.CodecType == "video");
      if(videoStream != null)
      {
        infoPartVideo.VideoHeight = videoStream.Height;
        infoPartVideo.VideoWidth = videoStream.Width;
        infoPartVideo.VideoCodecName = videoStream.CodecName;
        infoPartVideo.VideoCodecLongName = videoStream.CodecLongName;
      }

      // Format tags
      if(ffProbeOutput.Format.Tags != null && ffProbeOutput.Format.Tags.ContainsKey("title"))
      {
        infoPartVideo.Title = ffProbeOutput.Format.Tags["title"];
      }
    }
  }
}

