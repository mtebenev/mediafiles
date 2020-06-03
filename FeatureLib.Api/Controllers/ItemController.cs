using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Mt.MediaFiles.FeatureLib.Api.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mt.MediaFiles.FeatureLib.Api.Controllers
{
  /// <summary>
  /// The catalog items controller.
  /// </summary>
  public sealed class ItemController : WebApiController
  {
    [Route(HttpVerbs.Get, "/items")]
    public Task<List<CatalogItemDto>> Get()
    {
      var result = new List<CatalogItemDto>
      {
        new CatalogItemDto {CatalogItemId = 1, Path = "Path 1"},
        new CatalogItemDto {CatalogItemId = 2, Path = "Path 2"},
        new CatalogItemDto {CatalogItemId = 3, Path = "Path 3"},
      };

      return Task.FromResult(result);
    }
  }
}
