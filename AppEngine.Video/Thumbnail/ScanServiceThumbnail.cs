using MediaToolkit.Services;
using MediaToolkit.Tasks;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using Mt.MediaFiles.AppEngine.Scanning;
using Mt.MediaFiles.AppEngine.Video.Common;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Threading.Tasks;

namespace Mt.MediaFiles.AppEngine.Video.Thumbnail
{
  /// <summary>
  /// Saves media thumbnails during the scanning.
  /// </summary>
  internal class ScanServiceThumbnail : IScanService
  {
    private readonly IFileSystem _fileSystem;
    private readonly IThumbnailStorage _storage;
    private readonly IMediaToolkitService _mediaToolkitService;

    /// <summary>
    /// Ctor.
    /// </summary>
    public ScanServiceThumbnail(IFileSystem fileSystem, IThumbnailStorage storage, IMediaToolkitService mediaToolkitService)
    {
      this._fileSystem = fileSystem;
      this._storage = storage;
      this._mediaToolkitService = mediaToolkitService;
    }

    /// <summary>
    /// IScanService.
    /// </summary>
    public string Id => HandlerIds.ScanSvcThumbnail;

    /// <summary>
    /// IScanService.
    /// </summary>
    public IReadOnlyList<string> Dependencies => new string[0];

    /// <summary>
    /// IScanService.
    /// </summary>
    public Task FlushAsync()
    {
      return Task.CompletedTask;
    }

    /// <summary>
    /// IScanService.
    /// </summary>
    public async Task ScanAsync(IScanServiceContext context, CatalogItemRecord record)
    {
      if(FileExtensionCheck.IsVideo(this._fileSystem, record.Path))
      {
        var itemData = context.GetItemData();
        var infoPartVideo = itemData.Get<InfoPartVideo>();
        if (infoPartVideo != null)
        {
          var offsets = this.CalculateOffsets(infoPartVideo);
          var thumbnailRecords = await this.CreateThumbnailRecordsAsync(offsets, record);
          await this._storage.SaveRecordsAsync(thumbnailRecords);
        }
      }
    }

    /// <summary>
    /// Calculates offsets for thumbnails in the video.
    /// </summary>
    private int[] CalculateOffsets(InfoPartVideo infoPartVideo)
    {
      const int thumbnailCount = 6;
      var offsetIncrement = infoPartVideo.Duration / (thumbnailCount + 2); // 6 thumbnails + 2 marings.
      var result = new int[thumbnailCount];
      var currentOffset = offsetIncrement;
      for (int i = 0; i < thumbnailCount; i++)
      {
        result[i] = currentOffset;
        currentOffset += offsetIncrement;
      }

      return result;
    }

    private async Task<ThumbnailRecord[]> CreateThumbnailRecordsAsync(int[] offsets, CatalogItemRecord record)
    {
      var result = new ThumbnailRecord[offsets.Length];
      var options = new GetThumbnailOptions
      {
        FrameSize = new FrameSize(250, 250),
        OutputFormat = OutputFormat.Image2
      };

      for (int i = 0; i < offsets.Length; i++)
      {
        options.SeekSpan = TimeSpan.FromMilliseconds(offsets[i]);
        var thumbnailTask = new FfTaskGetThumbnail(record.Path, options);
        var taskResult = await this._mediaToolkitService.ExecuteAsync(thumbnailTask);
        var thumbnailRecord = new ThumbnailRecord
        {
          CatalogItemId = record.CatalogItemId,
          Offset = offsets[i],
          ThumbnailData = taskResult.ThumbnailData
        };
        result[i] = thumbnailRecord;
      }

      return result;
    }
  }
}
