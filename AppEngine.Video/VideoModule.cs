using AppEngine.Video.Comparison;
using AspNetCoreInjection.TypedFactories;
using Microsoft.Extensions.DependencyInjection;
using Mt.MediaFiles.AppEngine.Scanning;
using Mt.MediaFiles.AppEngine.Video.Tasks;
using Mt.MediaFiles.AppEngine.Video.Thumbnail;
using Mt.MediaFiles.AppEngine.Video.VideoImprint;

namespace Mt.MediaFiles.AppEngine.Video
{
  /// <summary>
  /// The video module.
  /// </summary>
  public class VideoModule
  {
    public static void ConfigureServices(IServiceCollection services)
    {
      // Catalog tasks
      services
        .RegisterTypedFactory<ICatalogTaskUpdateVideoImprintsFactory>().ForConcreteType<CatalogTaskUpdateVideoImprints>();
      services
        .RegisterTypedFactory<ICatalogTaskSearchVideoDuplicatesFactory>().ForConcreteType<CatalogTaskSearchVideoDuplicates>();
      services
        .RegisterTypedFactory<ICatalogTaskSearchVideoFactory>().ForConcreteType<CatalogTaskSearchVideo>();

      // Scan services
      services.AddTransient<IScanService, ScanServiceVideoImprint>();
      services.AddTransient<IScanService, ScanServiceThumbnail>();

      // Internals
      services
        .RegisterTypedFactory<IVideoImprintComparerFactory>().ForConcreteType<VideoImprintComparer>();
      services
        .RegisterTypedFactory<IVideoImprintUpdaterFactory>().ForConcreteType<VideoImprintUpdater>();
      services.AddTransient<IVideoImprintBuilder, VideoImprintBuilder>();
    }
  }
}
