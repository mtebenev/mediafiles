using System.Threading.Tasks;
using Mt.MediaFiles.AppEngine.Search;

namespace Mt.MediaFiles.AppEngine.FileHandlers
{
  /// <summary>
  /// Does not index anything
  /// </summary>
  internal class CatalogItemIndexerNull : ICatalogItemIndexer
  {
    public Task IndexItemAsync(IIndexingContext indexingContext)
    {
      return Task.CompletedTask;
    }
  }
}
