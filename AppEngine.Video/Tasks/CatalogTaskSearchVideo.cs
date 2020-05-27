using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppEngine.Video.Comparison;
using AppEngine.Video.VideoImprint;
using MediaToolkit.Services;
using MediaToolkit.Tasks;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.Matching;
using Mt.MediaFiles.AppEngine.Tasks;
using Mt.MediaFiles.AppEngine.Video.VideoImprint;

namespace Mt.MediaFiles.AppEngine.Video.Tasks
{
  public interface ICatalogTaskSearchVideoFactory
  {
    CatalogTaskBase<MatchResult> Create(IEnumerable<string> paths);
  }

  /// <summary>
  /// Searches a video in the catalog.
  /// </summary>
  public sealed class CatalogTaskSearchVideo : CatalogTaskBase<MatchResult>
  {
    private readonly ITaskExecutionContext _executionContext;
    private readonly IVideoImprintStorage _imprintStorage;
    private readonly IVideoImprintBuilder _imprintBuilder;
    private readonly IVideoImprintComparerFactory _comparerFactory;
    private readonly IMediaToolkitService _mediaToolkitService;
    private readonly IList<string> _paths;

    public CatalogTaskSearchVideo(
      ITaskExecutionContext executionContext,
      IVideoImprintStorage imprintStorage,
      IVideoImprintComparerFactory comparerFactory,
      IVideoImprintBuilder imprintBuilder,
      IMediaToolkitService mediaToolkitService,
      IEnumerable<string> paths
    )
    {
      this._executionContext = executionContext;
      this._imprintStorage = imprintStorage;
      this._imprintBuilder = imprintBuilder;
      this._comparerFactory = comparerFactory;
      this._mediaToolkitService = mediaToolkitService;
      this._paths = paths.ToList();
    }

    /// <summary>
    /// CatalogTaskBase.
    /// </summary>
    protected override async Task<MatchResult> ExecuteAsync(ICatalogContext catalogContext)
    {
      var imprintRecords = await this._imprintStorage.GetAllRecordsAsync();
      var matchGroups = new List<MatchResultGroup>();
      var fsImprints = await this.CreateFsImprintsAsync();
      var comparer = this._comparerFactory.Create();

      using(var progressOperation = this._executionContext.ProgressIndicator.StartOperation("Searching for videos..."))
      using(var taskProgress = progressOperation.CreateChildOperation(imprintRecords.Count))
      {
        for(var i = 0; i < fsImprints.Count; i++)
        {
          taskProgress.UpdateStatus(i.ToString());
          var mg = new MatchResultGroup(i);
          for(var j = 0; j < imprintRecords.Count; j++)
          {
            var isEqual = comparer.Compare(fsImprints[i].Item2.ImprintData, imprintRecords[j].ImprintData);
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

    private async Task<IList<(string, VideoImprintRecord)>> CreateFsImprintsAsync()
    {
      IList<(string, VideoImprintRecord)> result;
      using(this._executionContext.ProgressIndicator.StartOperation("Scanning videos..."))
      {
        result = await this._paths
          .ToAsyncEnumerable()
          .SelectAwait(async p =>
          {
            var videoDuration = await this.GetVideoDurationAsync(p);
            var r = await this._imprintBuilder.CreateRecordAsync(0, p, videoDuration);
            return (p, r);
          })
          .ToListAsync();
      }

      return result;
    }

    private async Task<double> GetVideoDurationAsync(string filePath)
    {
      var metadataTask = new FfTaskGetMetadata(filePath);
      var taskResult = await this._mediaToolkitService.ExecuteAsync(metadataTask);

      return taskResult.Metadata.Format.Duration.TotalMilliseconds;
    }
  }
}
