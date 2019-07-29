using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Scanning;

namespace AppEngine.Video.Discovery
{
  /// <summary>
  /// Iterates over media files.
  /// </summary>
  internal class MediaObjectIterator
  {
    private IAsyncEnumerable<ICatalogItem> _enumerable;

    public MediaObjectIterator(ICatalog catalog, int itemId)
    {
      this._enumerable = CatalogTreeWalker.CreateDefaultWalker(catalog, itemId);
    }

    /// <summary>
    /// Retrieves media object list starting from the item.
    /// </summary>
    public async Task<IList<IMediaObject>> GetMediaObjectsAsync()
    {
      var catalogItems = await this._enumerable.ToList();
      List<IMediaObject> result = new List<IMediaObject>();

      foreach(var item in catalogItems)
      {
        var videoPart = await item.GetInfoPartAsync<InfoPartVideo>();
        if(videoPart != null)
        {
          var mediaObject = new MediaObject(item);
          result.Add(mediaObject);
        }
      }

      return result;      
    }
  }
}
