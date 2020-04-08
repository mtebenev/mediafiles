using Mt.MediaFiles.AppEngine.CatalogStorage;
using YesSql.Indexes;

namespace Mt.MediaFiles.AppEngine.Ebooks.Storage
{
  /// <summary>
  /// Defines YesSql index: catalog item -> ebook
  /// </summary>
  internal class MapIndexEbook : MapIndex
  {
    public string EbookId { get; set; }
  }

  /// <summary>
  /// YesSql mapped index provider for ebooks
  /// </summary>
  internal class EbookIndexProvider : IndexProvider<CatalogItemData>
  {
    public override void Describe(DescribeContext<CatalogItemData> context)
    {
      context.For<MapIndexEbook>().Map(itemData =>
      {
        MapIndexEbook result = null;

        var ebookLink = itemData.Get<InfoPartEbookLink>();
        if(ebookLink != null)
          result = new MapIndexEbook { EbookId = ebookLink.EbookId };

        return result;
      });
    }
  }

}
