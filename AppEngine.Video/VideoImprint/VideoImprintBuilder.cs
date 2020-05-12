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
    private readonly AHash _ahash;

    public VideoImprintBuilder(IMediaToolkitService mediaToolkitService)
    {
      this._mediaToolkitService = mediaToolkitService;
      this._ahash = new AHash(AppEngineConstants.ImprintThumbnailSize);
    }

    /// <summary>
    /// IVideoImprintBuilder.
    /// </summary>
    public async Task<VideoImprintRecord> CreateRecordAsync(int catalogItemId, string fsPath)
    {
      var options = new GetThumbnailOptions
      {
        SeekSpan = TimeSpan.FromSeconds(5),
        FrameSize = new FrameSize(AppEngineConstants.ImprintThumbnailSize, AppEngineConstants.ImprintThumbnailSize),
        OutputFormat = OutputFormat.RawVideo,
        PixelFormat = PixelFormat.Gray
      };
      var thumbnailTask = new FfTaskGetThumbnail(fsPath, options);

      var taskResult = await this._mediaToolkitService.ExecuteAsync(thumbnailTask);
      var record = new VideoImprintRecord
      {
        CatalogItemId = catalogItemId,
        ImprintData = this._ahash.ComputeHash(taskResult.ThumbnailData)
      };
      return record;
    }
  }
}
