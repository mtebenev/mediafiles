using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using Mt.MediaFiles.AppEngine.Scanning;

namespace Mt.MediaFiles.AppEngine.Video.VideoImprint
{
  /// <summary>
  /// The video imprints scan service.
  /// </summary>
  internal class ScanServiceVideoImprint : IScanService
  {
    private readonly IVideoImprintUpdaterFactory _updaterFactory;

    /// <summary>
    /// Ctor.
    /// </summary>
    public ScanServiceVideoImprint(IVideoImprintUpdaterFactory updaterFactory)
    {
      this._updaterFactory = updaterFactory;
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
        var updater = this._updaterFactory.Create();
        await updater.UpdateAsync(record.CatalogItemId, record.Path);
      }
    }
  }
}
