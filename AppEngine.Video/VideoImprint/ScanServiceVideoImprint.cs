using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AppEngine.Video.VideoImprint;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using Mt.MediaFiles.AppEngine.Scanning;

namespace Mt.MediaFiles.AppEngine.Video.VideoImprint
{
  /// <summary>
  /// The video imprints scan service.
  /// Responsible for coordinating imprint storage and builder.
  /// </summary>
  internal class ScanServiceVideoImprint : IScanService
  {
    private readonly IVideoImprintBuilder _builder;
    private readonly IVideoImprintStorage _storage;
    private readonly VideoImprintRecord[] _buffer;
    private int _bufferPosition;

    /// <summary>
    /// Ctor.
    /// </summary>
    public ScanServiceVideoImprint(IVideoImprintBuilder videoImprintBuilder, IVideoImprintStorage videoImprintStorage, int bufferSize)
    {
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
    public async Task ScanAsync(IScanServiceContext context, CatalogItemRecord record)
    {
      var extension = Path.GetExtension(record.Path);
      var supportedExtensions = new[] { ".flv", ".mp4", ".wmv", ".avi", ".mkv" };
      if(supportedExtensions.Any(e => e.Equals(extension)))
      {
        var imprintRecord = await this._builder.CreateRecordAsync(record.CatalogItemId, record.Path);
        await this.SaveRecordAsync(imprintRecord);
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
