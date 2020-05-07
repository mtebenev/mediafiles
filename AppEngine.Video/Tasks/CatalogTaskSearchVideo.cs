using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppEngine.Video.Comparison;
using AppEngine.Video.VideoImprint;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.Tasks;
using Mt.MediaFiles.AppEngine.Tools;
using Mt.MediaFiles.AppEngine.Video.VideoImprint;

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
    private readonly IVideoImprintBuilder _imprintBuilder;
    private readonly IVideoImprintComparerFactory _comparerFactory;
    private readonly IList<string> _paths;

    public CatalogTaskSearchVideo(
      ITaskExecutionContext executionContext,
      IVideoImprintStorage imprintStorage,
      IVideoImprintComparerFactory comparerFactory,
      IVideoImprintBuilder imprintBuilder,
      IEnumerable<string> paths
    )
    {
      this._executionContext = executionContext;
      this._imprintStorage = imprintStorage;
      this._imprintBuilder = imprintBuilder;
      this._comparerFactory = comparerFactory;
      this._paths = paths.ToList();
    }

    /// <summary>
    /// CatalogTaskBase.
    /// </summary>
    protected override async Task<IList<DuplicateFindResult>> ExecuteAsync(ICatalogContext catalogContext)
    {
      var imprintRecords = await this._imprintStorage.GetAllRecordsAsync();
      var duplicateGroups = new List<IList<int>>();
      var fsImprints = await this.CreateFsImprintsAsync();
      var comparer = this._comparerFactory.Create();

      using(var progressOperation = this._executionContext.ProgressIndicator.StartOperation("Finding videos..."))
      using(var taskProgress = progressOperation.CreateChildOperation(imprintRecords.Count))
      {
        for(var i = 0; i < imprintRecords.Count; i++)
        {
          taskProgress.UpdateStatus(i.ToString());
          var duplicatedIds = new List<int>();
          for(var j = 0; j < fsImprints.Count; j++)
          {
            var isEqual = comparer.Compare(imprintRecords[i].ImprintData, fsImprints[j].Item2.ImprintData);
            if(isEqual)
            {
              duplicatedIds.Add(imprintRecords[i].CatalogItemId);
              duplicateGroups.Add(duplicatedIds);
            }
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

    private async Task<IList<(string, VideoImprintRecord)>> CreateFsImprintsAsync()
    {
      IList<(string, VideoImprintRecord)> result;
      using(this._executionContext.ProgressIndicator.StartOperation("Scanning videos..."))
      {
        result = await this._paths
          .ToAsyncEnumerable()
          .SelectAwait(async p =>
          {
            var r = await this._imprintBuilder.CreateRecordAsync(0, p);
            return (p, r);
          })
          .ToListAsync();
      }

      return result;
    }
  }
}
