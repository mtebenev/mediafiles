using Microsoft.Extensions.DependencyInjection;
using Mt.MediaMan.AppEngine.Cataloging;

namespace AppEngine.Video.VideoImprint
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
      services.AddSingleton<IVideoImprintStorage, VideoImprintStorage>();
    }
  }
}
