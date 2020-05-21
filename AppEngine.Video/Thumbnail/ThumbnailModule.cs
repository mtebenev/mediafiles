using Microsoft.Extensions.DependencyInjection;
using Mt.MediaFiles.AppEngine.Cataloging;

namespace Mt.MediaFiles.AppEngine.Video.Thumbnail
{
  /// <summary>
  /// The thumbnail module.
  /// </summary>
  public class ThumbnailModule
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
      services.AddSingleton<IThumbnailStorage, ThumbnailStorage>();
    }
  }
}
