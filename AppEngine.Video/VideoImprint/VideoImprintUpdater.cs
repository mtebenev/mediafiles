using System.Threading.Tasks;
using AppEngine.Video.VideoImprint;

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

    public VideoImprintUpdater(IVideoImprintStorage videoImprintStorage, IVideoImprintBuilder videoImprintBuilder)
    {
      this._videoImprintStorage = videoImprintStorage;
      this._videoImprintBuilder = videoImprintBuilder;
    }

    public async Task UpdateAsync(int catalogItemId, string fsPath)
    {
      await this._videoImprintStorage.DeleteRecordsAsync(catalogItemId);
      var imprintRecord = await this._videoImprintBuilder.CreateRecordAsync(catalogItemId, fsPath);
      await this._videoImprintStorage.SaveRecordsAsync(new[] { imprintRecord });
    }
  }
}
