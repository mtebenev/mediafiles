using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Cataloging;

namespace AppEngine.Video.VideoImprint
{
  /// <summary>
  /// Updates video imprint for a catalog item.
  /// </summary>
  internal class UpdateVideoImprintTask
  {
    private IVideoImprintStorage _videoImprintStorage;
    private ICatalogItem _catalogItem;
    private string _fsPath;

    public UpdateVideoImprintTask(IVideoImprintStorage videoImprintStorage, ICatalogItem catalogItem, string fsPath)
    {
      this._videoImprintStorage = videoImprintStorage;
      this._catalogItem = catalogItem;
      this._fsPath = fsPath;
    }

    public async Task ExecuteAsync()
    {
      var imprintRecord = await this.CreateRecordAsync(_catalogItem);
      await this._videoImprintStorage.SaveRecordAsync(imprintRecord);
    }

    private Task<VideoImprintRecord> CreateRecordAsync(ICatalogItem catalogItem)
    {
      var record = new VideoImprintRecord
      {
        CatalogItemId = catalogItem.CatalogItemId,
        ImprintData = new byte[100]
      };
      return Task.FromResult(record);
    }
  }
}
