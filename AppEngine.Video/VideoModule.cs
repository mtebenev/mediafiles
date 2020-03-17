using AppEngine.Video.Comparison;
using AspNetCoreInjection.TypedFactories;
using Microsoft.Extensions.DependencyInjection;

namespace AppEngine.Video.Test
{
  /// <summary>
  /// The video module.
  /// </summary>
  public class VideoModule
  {
    public static void ConfigureServices(IServiceCollection services)
    {
      services
        .RegisterTypedFactory<IVideoComparisonFactory>().ForConcreteType<VideoComparison>();
    }
  }
}
