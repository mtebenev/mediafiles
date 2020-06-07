using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.FeatureLib.Api.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mt.MediaFiles.FeatureLib.Api.Controllers
{
  /// <summary>
  /// The catalog items controller.
  /// </summary>
  public sealed class ItemController : WebApiController
  {
    private readonly ICatalogContext _catalogContext;

    /// <summary>
    /// Ctor.
    /// </summary>
    public ItemController(ICatalogContext catalogContext)
    {
      this._catalogContext = catalogContext;
    }

    /// <summary>
    /// Returns catalog items.
    /// </summary>
    [Route(HttpVerbs.Get, "/items")]
    public async Task<List<CatalogItemDto>> Get()
    {
      var walker = CatalogTreeWalker.CreateDefaultWalker(
        this._catalogContext.Catalog,
        this._catalogContext.Catalog.RootItem.CatalogItemId);

      var items = await walker.ToListAsync();
      var result = items.Select(i =>
      new CatalogItemDto
      {
        CatalogItemId = i.CatalogItemId,
        Path = i.Path
      }).ToList();

      return result;
    }
  }
}
