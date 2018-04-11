using YesSql.Indexes;

namespace Mt.MediaMan.AppEngine.CatalogStorage
{
  /// <summary>
  /// Defines YesSql index for catalog items
  /// </summary>
  internal class MapIndexCatalogItem : MapIndex
  {
    public int CatalogItemId { get; set; }
  }

  /// <summary>
  /// YesSql mapped index provider for catalog items
  /// </summary>
  internal class CatalogItemIndexProvider : IndexProvider<CatalogItemData>
  {
    public override void Describe(DescribeContext<CatalogItemData> context)
    {
      context.For<MapIndexCatalogItem>().Map(itemData => new MapIndexCatalogItem {CatalogItemId = itemData.CatalogItemId});
    }
  }

}
