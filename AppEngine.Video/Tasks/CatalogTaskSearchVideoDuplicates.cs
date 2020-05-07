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
    private readonly IVideoImprintComparerFactory _comparerFactory;

    public CatalogTaskSearchVideoDuplicates(IVideoImprintStorage imprintStorage, IVideoImprintComparerFactory comparerFactory)
    {
      this._imprintStorage = imprintStorage;
      this._comparerFactory = comparerFactory;
    }

    /// <summary>
    /// CatalogTaskBase.
    /// </summary>
    protected override async Task<IList<DuplicateFindResult>> ExecuteAsync(ICatalogContext catalogContext)
    {
      var imprintRecords = await this._imprintStorage.GetAllRecordsAsync();
      var duplicateGroups = new List<IList<int>>();

      for(var i = 0; i < imprintRecords.Count; i++)
      {
        var duplicatedIds = new List<int>() { imprintRecords[i].CatalogItemId };
        for(var j = i + 1; j < imprintRecords.Count; j++)
        {
          var comparisonTask = this._comparerFactory.Create();
          var isEqual = comparisonTask.Compare(imprintRecords[i].ImprintData, imprintRecords[j].ImprintData);
          if(isEqual)
          {
            duplicatedIds.Add(imprintRecords[j].CatalogItemId);
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
