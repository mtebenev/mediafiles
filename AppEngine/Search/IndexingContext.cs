using Mt.MediaMan.AppEngine.CatalogStorage;

namespace Mt.MediaMan.AppEngine.Search
{
  internal class IndexingContext : IIndexingContext
  {
    public IndexingContext(DocumentIndex documentIndex, CatalogItemRecord itemRecord, CatalogItemData catalogItemData)
    {
      ItemRecord = itemRecord;
      CatalogItemData = catalogItemData;
      DocumentIndex = documentIndex;
    }

    public DocumentIndex DocumentIndex { get; }
    public CatalogItemRecord ItemRecord { get; }
    public CatalogItemData CatalogItemData { get; }
  }
}
