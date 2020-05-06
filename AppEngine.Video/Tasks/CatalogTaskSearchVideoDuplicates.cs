using System.Collections.Generic;
using System.Threading.Tasks;
using AppEngine.Video.Comparison;
using AppEngine.Video.VideoImprint;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.Tasks;
using Mt.MediaFiles.AppEngine.Tools;

namespace Mt.MediaFiles.AppEngine.Video.Tasks
{
  public interface ICatalogTaskSearchVideoDuplicatesFactory
  {
    public CatalogTaskBase<IList<DuplicateFindResult>> Create();
  }

  /// <summary>
  /// Searches for video duplicates in the catalog.
  /// </summary>
  public sealed class CatalogTaskSearchVideoDuplicates : CatalogTaskBase<IList<DuplicateFindResult>>
  {
    private readonly IVideoImprintStorage _imprintStorage;
    private readonly IVideoComparerFactory _videoComparisonFactory;

    public CatalogTaskSearchVideoDuplicates(IVideoImprintStorage imprintStorage, IVideoComparerFactory videoComparisonFactory)
    {
      this._imprintStorage = imprintStorage;
      this._videoComparisonFactory = videoComparisonFactory;
    }

    /// <summary>
    /// CatalogTaskBase.
    /// </summary>
    protected override async Task<IList<DuplicateFindResult>> ExecuteAsync(ICatalogContext catalogContext)
    {
      var itemIds = await this._imprintStorage.GetCatalogItemIdsAsync();
      var duplicateGroups = new List<IList<int>>();

      for(var i = 0; i < itemIds.Count; i++)
      {
        var duplicatedIds = new List<int>() { itemIds[i] };
        for(var j = i + 1; j < itemIds.Count; j++)
        {
          var comparisonTask = this._videoComparisonFactory.Create();
          var isEqual = await comparisonTask.CompareItemsAsync(itemIds[i], itemIds[j]);
          if(isEqual)
          {
            duplicatedIds.Add(itemIds[j]);
          }
        }
        if(duplicatedIds.Count > 1)
        {
          duplicateGroups.Add(duplicatedIds);
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
