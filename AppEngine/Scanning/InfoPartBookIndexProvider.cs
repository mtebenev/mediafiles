using Mt.MediaMan.AppEngine.CatalogStorage;
using YesSql.Indexes;

namespace Mt.MediaMan.AppEngine.Scanning
{
  /// <summary>
  /// YesSql mapped index provider for catalog items
  /// </summary>
  internal class InfoPartBookIndexProvider : IndexProvider<InfoPartBook>
  {
    public override void Describe(DescribeContext<InfoPartBook> context)
    {
      context.For<MapIndexCatalogItem>().Map(infoPartBook => new MapIndexCatalogItem {CatalogItemId = infoPartBook.CatalogItemId});
    }
  }
}
