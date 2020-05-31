using AppEngine.Video.VideoImprint;
using Microsoft.Extensions.DependencyInjection;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using System.Data;

namespace Mt.MediaFiles.AppEngine.Video.VideoImprint
{
  /// <summary>
  /// The video imprint module.
  /// </summary>
  public class VideoImprintModule
  {
    /// <summary>
    /// Call to configure the storage.
    /// </summary>
    public static void ConfigureStorage(StorageConfiguration storageConfiguration)
    {
      var moduleDbProvider = new ModuleDbProvider();
      storageConfiguration.AddModuleDbProvider(moduleDbProvider);
    }

    /// <summary>
    /// Call to configure the container.
    /// </summary>
    public static void ConfigureContainer(IServiceCollection services)
    {
      const int ImprintBufferSize = 1000;
      services.AddSingleton<VideoImprintStorage>(
        c => new VideoImprintStorage(
          c.GetRequiredService<IDbConnection>(),
          ImprintBufferSize
      ));
      services.AddSingleton<IBufferedStorage>(c => c.GetRequiredService<VideoImprintStorage>());
      services.AddSingleton<IVideoImprintStorage>(c => c.GetRequiredService<VideoImprintStorage>());
    }
  }
}
