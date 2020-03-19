using AppEngine.Video.Comparison;
using AppEngine.Video.VideoImprint;
using AspNetCoreInjection.TypedFactories;
using Microsoft.Extensions.DependencyInjection;
using Mt.MediaMan.AppEngine.Video.Tasks;

namespace AppEngine.Video.Test
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
        .RegisterTypedFactory<IUpdateVideoImprintsFactory>().ForConcreteType<UpdateVideoImprints>();

      // Internals
      services
        .RegisterTypedFactory<IVideoComparisonFactory>().ForConcreteType<VideoComparison>();
      services
        .RegisterTypedFactory<IVideoImprintUpdaterFactory>().ForConcreteType<VideoImprintUpdater>();
    }
  }
}
