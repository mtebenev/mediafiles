using System;
using System.Threading.Tasks;
using AppEngine.Video.VideoImprint;
using MediaToolkit.Services;
using MediaToolkit.Tasks;

namespace Mt.MediaFiles.AppEngine.Video.VideoImprint
{
  public interface IVideoImprintUpdaterFactory
  {
    internal IVideoImprintUpdater Create();
  }

  internal interface IVideoImprintUpdater
  {
    Task UpdateAsync(int catalogItemId, string fsPath);
  }

  /// <summary>
  /// Updates video imprint for a catalog item.
  /// </summary>
  internal class VideoImprintUpdater : IVideoImprintUpdater
  {
    private IVideoImprintStorage _videoImprintStorage;
    private readonly IMediaToolkitService _mediaToolkitService;

    public VideoImprintUpdater(IVideoImprintStorage videoImprintStorage, IMediaToolkitService mediaToolkitService)
    {
      this._videoImprintStorage = videoImprintStorage;
      this._mediaToolkitService = mediaToolkitService;
    }

    public async Task UpdateAsync(int catalogItemId, string fsPath)
    {
      await this._videoImprintStorage.DeleteRecordsAsync(catalogItemId);
      var imprintRecord = await this.CreateRecordAsync(catalogItemId, fsPath);
      await this._videoImprintStorage.SaveRecordAsync(imprintRecord);
    }

    private async Task<VideoImprintRecord> CreateRecordAsync(int catalogItemId, string fsPath)
    {
      var options = new GetThumbnailOptions
      {
        SeekSpan = TimeSpan.FromSeconds(1),
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
