using System;
using System.Threading.Tasks;
using MediaToolkit.Services;
using MediaToolkit.Tasks;
using Mt.MediaMan.AppEngine.Cataloging;

namespace AppEngine.Video.VideoImprint
{
  /// <summary>
  /// Updates video imprint for a catalog item.
  /// </summary>
  internal class UpdateVideoImprintTask
  {
    private IVideoImprintStorage _videoImprintStorage;
    private readonly IMediaToolkitService _mediaToolkitService;
    private ICatalogItem _catalogItem;
    private string _fsPath;

    public UpdateVideoImprintTask(IVideoImprintStorage videoImprintStorage, IMediaToolkitService mediaToolkitService, ICatalogItem catalogItem, string fsPath)
    {
      this._videoImprintStorage = videoImprintStorage;
      this._mediaToolkitService = mediaToolkitService;
      this._catalogItem = catalogItem;
      this._fsPath = fsPath;
    }

    public async Task ExecuteAsync()
    {
      var imprintRecord = await this.CreateRecordAsync(_catalogItem);
      await this._videoImprintStorage.SaveRecordAsync(imprintRecord);
    }

    private async Task<VideoImprintRecord> CreateRecordAsync(ICatalogItem catalogItem)
    {
      var thumbnailTask = new FfTaskGetThumbnail(
        this._fsPath,
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
