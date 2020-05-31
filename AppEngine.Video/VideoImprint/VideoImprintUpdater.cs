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
    private readonly IVideoImprintStorage _videoImprintStorage;
    private readonly IVideoImprintBuilder _videoImprintBuilder;
    private readonly IMediaToolkitService _mediaToolkitService;

    public VideoImprintUpdater(
      IVideoImprintStorage videoImprintStorage,
      IVideoImprintBuilder videoImprintBuilder,
      IMediaToolkitService mediaToolkitService)
    {
      this._videoImprintStorage = videoImprintStorage;
      this._videoImprintBuilder = videoImprintBuilder;
      this._mediaToolkitService = mediaToolkitService;
    }

    public async Task UpdateAsync(int catalogItemId, string fsPath)
    {
      var metadataTask = new FfTaskGetMetadata(fsPath);
      var taskResult = await this._mediaToolkitService.ExecuteAsync(metadataTask);

      var totalDuration = taskResult.Metadata.Format.Duration.TotalMilliseconds;

      await this._videoImprintStorage.DeleteRecordsAsync(catalogItemId);
      var imprintRecord = await this._videoImprintBuilder.CreateRecordAsync(catalogItemId, fsPath, totalDuration / 2);
      await this._videoImprintStorage.SaveRecordAsync(imprintRecord);
    }
  }
}
