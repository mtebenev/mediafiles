using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Scanning;
using Mt.MediaMan.AppEngine.Search;

namespace Mt.MediaMan.AppEngine.FileHandlers
{
  /// <summary>
  /// Base class for all info part index handlers
  /// </summary>
  internal abstract class InfoPartIndexHandlerBase<TInfoPart> where TInfoPart : InfoPartBase
  {
    public abstract Task OnBuildIndexAsync(TInfoPart part, IIndexingContext indexingContext);
  }
}
