using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Mt.MediaFiles.AppEngine.Video.Thumbnail;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mt.MediaFiles.FeatureLib.Api.Controllers
{
  /// <summary>
  /// The thumbnails controller.
  /// </summary>
  public sealed class ThumbnailController : WebApiController
  {
    private readonly IThumbnailStorage _thumbnailStorage;

    /// <summary>
    /// Ctor.
    /// </summary>
    public ThumbnailController(IThumbnailStorage thumbnailStorage)
    {
      this._thumbnailStorage = thumbnailStorage;
    }

    /// <summary>
    /// Returns item thumbnails IDs.
    /// </summary>
    [Route(HttpVerbs.Get, "/item/{catalogItemId}/thumbnails")]
    public Task<IList<int>> GetThumbnailIds(int catalogItemId)
    {
      return this._thumbnailStorage.GetThumbnailIds(catalogItemId);
    }

    /// <summary>
    /// Returns catalog items.
    /// </summary>
    [Route(HttpVerbs.Get, "/thumbnails/{thumbnailId}")]
    public async Task GetAsync(int thumbnailId)
    {
      HttpContext.Response.ContentType = "image/jpeg";
      var data = await this._thumbnailStorage.GetThumbnailDataAsync(thumbnailId);
      using(var responseStream = HttpContext.OpenResponseStream(false, false))
      {
        responseStream.Write(data);
      }
    }
  }
}
