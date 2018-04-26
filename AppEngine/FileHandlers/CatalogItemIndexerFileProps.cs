using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Search;

namespace Mt.MediaMan.AppEngine.FileHandlers
{
  /// <summary>
  /// Indexes file properties
  /// </summary>
  internal class CatalogItemIndexerFileProps : ICatalogItemIndexer
  {
    /// <summary>
    /// ICatalogItemIndexer
    /// </summary>
    public Task IndexItemAsync(IIndexingContext indexingContext)
    {
      indexingContext.DocumentIndex.Entries.Add(
        "CatalogItem.Name",
        new DocumentIndexEntry(
          indexingContext.ItemRecord.Name,
          IndexValueType.Text,
          DocumentIndexOptions.Store));

      return Task.CompletedTask;
    }
  }
}
