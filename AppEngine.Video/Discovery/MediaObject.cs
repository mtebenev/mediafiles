using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Cataloging;

namespace AppEngine.Video.Discovery
{
  /// <summary>
  /// Media object implementation
  /// </summary>
  public class MediaObject : IMediaObject
  {
    private ICatalogItem _catalogItem;
    public MediaObject(ICatalogItem catalogItem)
    {
      this._catalogItem = catalogItem;
    }

    /// <summary>
    /// IMediaObject
    /// </summary>
    public Task<string> GetFsPathAsync()
    {
      return Task.FromResult(this._catalogItem.Path);
    }
  }
}
