using YesSql.Indexes;

namespace Mt.MediaMan.AppEngine.CatalogStorage
{
  /// <summary>
  /// Defines YesSql index for catalog items
  /// </summary>
  public class MapIndexCatalogItem : MapIndex
  {
    public int CatalogItemId { get; set; }
  }
}
