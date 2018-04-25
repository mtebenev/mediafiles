using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Search;

namespace Mt.MediaMan.AppEngine.FileHandlers
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
