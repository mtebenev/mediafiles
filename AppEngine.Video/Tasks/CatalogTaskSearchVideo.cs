using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppEngine.Video.Comparison;
using AppEngine.Video.VideoImprint;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.Tasks;
using Mt.MediaFiles.AppEngine.Tools;

namespace Mt.MediaFiles.AppEngine.Video.Tasks
{
  public interface ICatalogTaskSearchVideoFactory
  {
    CatalogTaskBase<IList<DuplicateFindResult>> Create(IEnumerable<string> paths);
  }

  /// <summary>
  /// Searches a video in the catalog.
  /// </summary>
  public sealed class CatalogTaskSearchVideo : CatalogTaskBase<IList<DuplicateFindResult>>
  {
    private readonly ITaskExecutionContext _executionContext;
    private readonly IVideoImprintStorage _imprintStorage;
    private readonly IVideoComparerFactory _comparerFactory;
    private readonly IList<string> _paths;

    public CatalogTaskSearchVideo(
      ITaskExecutionContext executionContext,
      IVideoImprintStorage imprintStorage,
      IVideoComparerFactory comparerFactory,
      IEnumerable<string> paths
    )
    {
      this._executionContext = executionContext;
      this._imprintStorage = imprintStorage;
      this._comparerFactory = comparerFactory;
      this._paths = paths.ToList();
    }

    /// <summary>
    /// CatalogTaskBase.
    /// </summary>
    protected override async Task<IList<DuplicateFindResult>> ExecuteAsync(ICatalogContext catalogContext)
    {
      var itemIds = await this._imprintStorage.GetCatalogItemIdsAsync();
      var duplicateGroups = new List<IList<int>>();

      using(var progressOperation = this._executionContext.ProgressIndicator.StartOperation("Finding videos..."))
      using(var taskProgress = progressOperation.CreateChildOperation(itemIds.Count))
      {
        for(var i = 0; i < itemIds.Count; i++)
        {
          taskProgress.UpdateStatus(i.ToString());
          var duplicatedIds = new List<int>() { itemIds[i] };
          for(var j = 0; j < this._paths.Count; j++)
          {
            var comparer = this._comparerFactory.Create();
            var isEqual = await comparer.CompareFsVideo(this._paths[j], itemIds[i]);
            if(isEqual)
            {
              duplicatedIds.Add(itemIds[i]);
            }
          }
          if(duplicatedIds.Count > 1)
          {
            duplicateGroups.Add(duplicatedIds);
          }
        }
      }

      var result = await this.CreateResultAsync(catalogContext, duplicateGroups);
      return result;
    }

    private async Task<IList<DuplicateFindResult>> CreateResultAsync(ICatalogContext catalogContext, IList<IList<int>> duplicateGroups)
    {
      var result = new List<DuplicateFindResult>();
      foreach(var group in duplicateGroups)
      {
        var r = await DuplicateFindResult.CreateAsync(catalogContext.Catalog, group);
        result.Add(r);
      }

      return result;
    }
  }
}
