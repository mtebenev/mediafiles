using System.Collections.Generic;
using System.Threading.Tasks;
using AppEngine.Video.Comparison;
using AppEngine.Video.VideoImprint;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Tools;

namespace Mt.MediaMan.AppEngine.Tasks
{
  /// <summary>
  /// Finds the video duplicates.
  /// </summary>
  public class FindVideoDuplicates : CatalogTaskBase<IList<DuplicateFindResult>>
  {
    private readonly IVideoImprintStorage _imprintStorage;
    private readonly IVideoComparisonFactory _videoComparisonFactory;

    public FindVideoDuplicates(IVideoImprintStorage imprintStorage, IVideoComparisonFactory videoComparisonFactory)
    {
      this._imprintStorage = imprintStorage;
      this._videoComparisonFactory = videoComparisonFactory;
    }

    public override async Task<IList<DuplicateFindResult>> ExecuteAsync(ICatalogContext catalogContext)
    {
      var itemIds = await this._imprintStorage.GetCatalogItemIdsAsync();
      var duplicateGroups = new List<IList<int>>();

      for(int i = 0; i < itemIds.Count; i++)
      {
        var duplicatedIds = new List<int>() { itemIds[i] };
        for(int j = i + 1; j < itemIds.Count; j++)
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
