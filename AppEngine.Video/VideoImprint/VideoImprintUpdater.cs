using System;
using System.Threading.Tasks;
using MediaToolkit.Services;
using MediaToolkit.Tasks;
using Mt.MediaMan.AppEngine.Cataloging;

namespace AppEngine.Video.VideoImprint
{
  public interface IVideoImprintUpdaterFactory
  {
    internal IVideoImprintUpdater Create();
  }

  internal interface IVideoImprintUpdater
  {
    Task UpdateAsync(ICatalogItem catalogItem, string fsPath);
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

    public async Task UpdateAsync(ICatalogItem catalogItem, string fsPath)
    {
      var imprintRecord = await this.CreateRecordAsync(catalogItem, fsPath);
      await this._videoImprintStorage.SaveRecordAsync(imprintRecord);
    }

    private async Task<VideoImprintRecord> CreateRecordAsync(ICatalogItem catalogItem, string fsPath)
    {
      var thumbnailTask = new FfTaskGetThumbnail(
        fsPath,
        TimeSpan.FromSeconds(1),
        new FrameSize(32, 32));

      var taskResult = await this._mediaToolkitService.ExecuteAsync(thumbnailTask);
      var record = new VideoImprintRecord
      {
        CatalogItemId = catalogItem.CatalogItemId,
        ImprintData = taskResult.ThumbnailData
      };
      return record;
    }
  }
}
