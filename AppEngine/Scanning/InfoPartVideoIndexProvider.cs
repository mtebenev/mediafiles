using Mt.MediaMan.AppEngine.CatalogStorage;
using YesSql.Indexes;

namespace Mt.MediaMan.AppEngine.Scanning
{
  /// <summary>
  /// YesSql mapped index provider for catalog items
  /// </summary>
  internal class InfoPartVideoIndexProvider : IndexProvider<InfoPartVideo>
  {
    public override void Describe(DescribeContext<InfoPartVideo> context)
    {
      context.For<MapIndexCatalogItem>().Map(infoPartVideo => new MapIndexCatalogItem {CatalogItemId = infoPartVideo.CatalogItemId});
    }
  }
}
