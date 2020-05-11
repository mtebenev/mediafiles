using System;
using System.Threading.Tasks;
using AppEngine.Video.VideoImprint;
using MediaToolkit.Services;
using MediaToolkit.Tasks;

namespace Mt.MediaFiles.AppEngine.Video.VideoImprint
{
  /// <summary>
  /// The video imprint builder implementation.
  /// </summary>
  internal class VideoImprintBuilder : IVideoImprintBuilder
  {
    private readonly IMediaToolkitService _mediaToolkitService;

    public VideoImprintBuilder(IMediaToolkitService mediaToolkitService)
    {
      this._mediaToolkitService = mediaToolkitService;
    }

    /// <summary>
    /// IVideoImprintBuilder.
    /// </summary>
    public async Task<VideoImprintRecord> CreateRecordAsync(int catalogItemId, string fsPath)
    {
      var options = new GetThumbnailOptions
      {
        SeekSpan = TimeSpan.FromSeconds(5),
        FrameSize = new FrameSize(32, 32),
        OutputFormat = OutputFormat.RawVideo,
        PixelFormat = PixelFormat.Gray
      };
      var thumbnailTask = new FfTaskGetThumbnail(fsPath, options);

      var taskResult = await this._mediaToolkitService.ExecuteAsync(thumbnailTask);
      var record = new VideoImprintRecord
      {
        CatalogItemId = catalogItemId,
        ImprintData = taskResult.ThumbnailData
      };
      return record;
    }
  }
}
