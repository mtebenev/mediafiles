using AppEngine.Video.Comparison;
using AppEngine.Video.VideoImprint;
using AspNetCoreInjection.TypedFactories;
using Microsoft.Extensions.DependencyInjection;
using Mt.MediaFiles.AppEngine.Scanning;
using Mt.MediaFiles.AppEngine.Video.Tasks;
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
      const int ImprintBufferSize = 1000;

      // Catalog tasks
      services
        .RegisterTypedFactory<ICatalogTaskUpdateVideoImprintsFactory>().ForConcreteType<CatalogTaskUpdateVideoImprints>();
      services
        .RegisterTypedFactory<ICatalogTaskFindVideoDuplicatesFactory>().ForConcreteType<CatalogTaskFindVideoDuplicates>();

      // Scan services
      services.AddTransient<IScanService>(
        c => new ScanServiceVideoImprint(
          c.GetRequiredService<IVideoImprintBuilder>(),
          c.GetRequiredService<IVideoImprintStorage>(),
          ImprintBufferSize
      ));

      // Internals
      services
        .RegisterTypedFactory<IVideoComparerFactory>().ForConcreteType<VideoComparer>();
      services
        .RegisterTypedFactory<IVideoImprintUpdaterFactory>().ForConcreteType<VideoImprintUpdater>();
      services.AddTransient<IVideoImprintBuilder, VideoImprintBuilder>();
    }
  }
}
