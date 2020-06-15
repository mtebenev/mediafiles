using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using AppEngine.Video.Comparison;
using AppEngine.Video.VideoImprint;
using MediaToolkit.Services;
using MediaToolkit.Tasks;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.Matching;
using Mt.MediaFiles.AppEngine.Tasks;
using Mt.MediaFiles.AppEngine.Video.Common;
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
    private readonly IFileSystem _fileSystem;
    private readonly IVideoImprintStorage _imprintStorage;
    private readonly IVideoImprintBuilder _imprintBuilder;
    private readonly IVideoImprintComparerFactory _comparerFactory;
    private readonly IMediaToolkitService _mediaToolkitService;
    private readonly IList<string> _paths;

    public CatalogTaskSearchVideo(
      ITaskExecutionContext executionContext,
      IFileSystem fileSystem,
      IVideoImprintStorage imprintStorage,
      IVideoImprintComparerFactory comparerFactory,
      IVideoImprintBuilder imprintBuilder,
      IMediaToolkitService mediaToolkitService,
      IEnumerable<string> paths
    )
    {
      this._executionContext = executionContext;
      this._fileSystem = fileSystem;
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

      this._executionContext.UpdateStatus("Searching for videos...");
      using(var progressOperation = this._executionContext.StartProgressOperation(fsImprints.Count))
      {
        for(var i = 0; i < fsImprints.Count; i++)
        {
          progressOperation.Tick();
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
        await progressOperation.Finish();
      }
      var result = new MatchResult(matchGroups);
      return result;
    }

    private async Task<IList<(string, VideoImprintRecord)>> CreateFsImprintsAsync()
    {
      IList<(string, VideoImprintRecord)> result;
      this._executionContext.UpdateStatus("Scanning videos...");

      var videoPaths = this._paths
        .Where(p => FileExtensionCheck.IsVideo(this._fileSystem, p))
        .ToList();

      using(var progressOperation = this._executionContext.StartProgressOperation(videoPaths.Count))
      {
        result = await videoPaths
        .ToAsyncEnumerable()
        .SelectAwait(async p =>
        {
          var videoDuration = await this.GetVideoDurationAsync(p);
          var r = await this._imprintBuilder.CreateRecordAsync(0, p, videoDuration);
          progressOperation.Tick();
          return (p, r);
        })
        .ToListAsync();
        await progressOperation.Finish();
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
