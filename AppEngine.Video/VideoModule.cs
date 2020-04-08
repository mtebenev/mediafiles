using AppEngine.Video.Comparison;
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
      // Catalog tasks
      services
        .RegisterTypedFactory<ICatalogTaskUpdateVideoImprintsFactory>().ForConcreteType<CatalogTaskUpdateVideoImprints>();
      services
        .RegisterTypedFactory<ICatalogTaskFindVideoDuplicatesFactory>().ForConcreteType<CatalogTaskFindVideoDuplicates>();

      // Scan services
      services.AddTransient<IScanService, ScanServiceVideoImprint>();

      // Internals
      services
        .RegisterTypedFactory<IVideoComparisonFactory>().ForConcreteType<VideoComparison>();
      services
        .RegisterTypedFactory<IVideoImprintUpdaterFactory>().ForConcreteType<VideoImprintUpdater>();
    }
  }
}
