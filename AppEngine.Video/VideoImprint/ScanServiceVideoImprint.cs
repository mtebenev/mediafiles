using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using AppEngine.Video.VideoImprint;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using Mt.MediaFiles.AppEngine.Scanning;
using Mt.MediaFiles.AppEngine.Video.Common;

namespace Mt.MediaFiles.AppEngine.Video.VideoImprint
{
  /// <summary>
  /// The video imprints scan service.
  /// Responsible for coordinating imprint storage and builder.
  /// Threading: not thread-safe.
  /// </summary>
  internal class ScanServiceVideoImprint : IScanService
  {
    private readonly IFileSystem _fileSystem;
    private readonly IVideoImprintBuilder _builder;
    private readonly IVideoImprintStorage _storage;
    private readonly VideoImprintRecord[] _buffer;
    private int _bufferPosition;

    /// <summary>
    /// Ctor.
    /// </summary>
    public ScanServiceVideoImprint(IFileSystem fileSystem, IVideoImprintBuilder videoImprintBuilder, IVideoImprintStorage videoImprintStorage, int bufferSize)
    {
      this._fileSystem = fileSystem;
      this._builder = videoImprintBuilder;
      this._storage = videoImprintStorage;
      this._buffer = new VideoImprintRecord[bufferSize];
      this._bufferPosition = 0;
    }

    /// <summary>
    /// IScanService.
    /// </summary>
    public string Id => AppEngine.Video.HandlerIds.ScanSvcVideoImprints;

    /// <summary>
    /// IScanService.
    /// </summary>
    public IReadOnlyList<string> Dependencies => new[] { AppEngine.HandlerIds.ScanSvcScanInfo };

    /// <summary>
    /// IScanService.
    /// </summary>
    public async Task ScanAsync(IScanServiceContext context, CatalogItemRecord record)
    {
      if(FileExtensionCheck.IsVideo(this._fileSystem, record.Path))
      {
        var itemData = context.GetItemData();
        var infoPartVideo = itemData.Get<InfoPartVideo>();
        if(infoPartVideo != null)
        {
          var imprintRecord = await this._builder.CreateRecordAsync(record.CatalogItemId, record.Path, infoPartVideo.Duration);
          await this.SaveRecordAsync(imprintRecord);
        }
      }
    }

    /// <summary>
    /// IScanService.
    /// </summary>
    public async Task FlushAsync()
    {
      if(this._bufferPosition > 0)
      {
        var toSave = this._buffer.Take(this._bufferPosition);
        await this._storage.SaveRecordsAsync(toSave);
        this._bufferPosition = 0;
      }
    }

    private async Task SaveRecordAsync(VideoImprintRecord imprintRecord)
    {
      if(this._bufferPosition == this._buffer.Length)
      {
        await this.FlushAsync();
      }

      this._buffer[this._bufferPosition] = imprintRecord;
      this._bufferPosition++;
    }
  }
}
