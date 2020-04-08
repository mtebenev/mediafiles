using System.Threading.Tasks;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using Mt.MediaFiles.AppEngine.Scanning;
using Mt.MediaFiles.AppEngine.Search;

namespace Mt.MediaFiles.AppEngine.FileHandlers
{
  /// <summary>
  /// Base class for all info part indexers
  /// </summary>
  internal abstract class InfoPartIndexerBase<TInfoPart> : ICatalogItemIndexer where TInfoPart : InfoPartBase
  {
    /// <summary>
    /// ICatalogIndexer
    /// </summary>
    public Task IndexItemAsync(IIndexingContext indexingContext)
    {
      var infoPart = indexingContext.CatalogItemData.Get<TInfoPart>();
      var result = OnBuildIndexAsync(infoPart, indexingContext);

      return result;
    }

    public abstract Task OnBuildIndexAsync(TInfoPart part, IIndexingContext indexingContext);
  }
}
