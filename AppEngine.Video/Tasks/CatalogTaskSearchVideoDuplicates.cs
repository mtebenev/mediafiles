using System.Collections.Generic;
using System.Threading.Tasks;
using AppEngine.Video.Comparison;
using AppEngine.Video.VideoImprint;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.Matching;
using Mt.MediaFiles.AppEngine.Tasks;

namespace Mt.MediaFiles.AppEngine.Video.Tasks
{
  public interface ICatalogTaskSearchVideoDuplicatesFactory
  {
    public CatalogTaskBase<MatchResult> Create();
  }

  /// <summary>
  /// Searches for video duplicates in the catalog.
  /// </summary>
  public sealed class CatalogTaskSearchVideoDuplicates : CatalogTaskBase<MatchResult>
  {
    private readonly ITaskExecutionContext _executionContext;
    private readonly IVideoImprintStorage _imprintStorage;
    private readonly IVideoImprintComparerFactory _comparerFactory;

    public CatalogTaskSearchVideoDuplicates(
      ITaskExecutionContext executionContext,
      IVideoImprintStorage imprintStorage,
      IVideoImprintComparerFactory comparerFactory
      )
    {
      this._executionContext = executionContext;
      this._imprintStorage = imprintStorage;
      this._comparerFactory = comparerFactory;
    }

    /// <summary>
    /// CatalogTaskBase.
    /// </summary>
    protected override async Task<MatchResult> ExecuteAsync(ICatalogContext catalogContext)
    {
      var matchGroups = new List<MatchResultGroup>();
      var comparisonTask = this._comparerFactory.Create();

      this._executionContext.UpdateStatus("Searching for videos...");
      var imprintRecords = await this._imprintStorage.GetAllRecordsAsync();

      using(var progressOperation = this._executionContext.StartProgressOperation(imprintRecords.Count))
      {
        for(var i = 0; i < imprintRecords.Count; i++)
        {
          progressOperation.Tick();
          var mg = new MatchResultGroup(imprintRecords[i].CatalogItemId);
          for(var j = i + 1; j < imprintRecords.Count; j++)
          {
            var isEqual = comparisonTask.Compare(imprintRecords[i].ImprintData, imprintRecords[j].ImprintData);
            if(isEqual)
            {
              mg.AddItemId(imprintRecords[j].CatalogItemId);
            }
          }
          if(mg.ItemIds.Count > 0)
          {
            matchGroups.Add(mg);
          }
        }
      }
      var result = new MatchResult(matchGroups);
      return result;
    }
  }
}
