using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Threading.Tasks;
using AppEngine.Video.VideoImprint;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using Mt.MediaFiles.AppEngine.Scanning;
using Mt.MediaFiles.AppEngine.Video.Common;

namespace Mt.MediaFiles.AppEngine.Video.VideoImprint
{
  /// <summary>
  /// The service factory.
  /// </summary>
  internal class ScanServiceFactoryVideoImprint : ScanServiceFactoryBase<ScanServiceVideoImprint>
  {
    public ScanServiceFactoryVideoImprint(IServiceProvider serviceProvider
      ) : base(
        serviceProvider,
        AppEngine.Video.HandlerIds.ScanSvcVideoImprints,
        new List<string> { AppEngine.HandlerIds.ScanSvcScanInfo })
    {
    }
  }

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

    /// <summary>
    /// Ctor.
    /// </summary>
    public ScanServiceVideoImprint(IFileSystem fileSystem, IVideoImprintBuilder videoImprintBuilder, IVideoImprintStorage videoImprintStorage)
    {
      this._fileSystem = fileSystem;
      this._builder = videoImprintBuilder;
      this._storage = videoImprintStorage;
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
      if(FileExtensionCheck.IsVideo(this._fileSystem, record.Path))
      {
        var itemData = context.GetItemData();
        var infoPartVideo = itemData.Get<InfoPartVideo>();
        if(infoPartVideo != null)
        {
          var imprintRecord = await this._builder.CreateRecordAsync(record.CatalogItemId, record.Path, infoPartVideo.Duration);
          await this._storage.SaveRecordAsync(imprintRecord);
        }
      }
    }
  }
}
