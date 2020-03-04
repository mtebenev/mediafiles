using Mt.MediaMan.AppEngine.Cataloging;

namespace AppEngine.Video.VideoImprint
{
  /// <summary>
  /// The video imprint module.
  /// </summary>
  public class VideoImprintModule
  {
    public static void CreateStorageConfiguration(StorageConfiguration storageConfiguration)
    {
      var moduleDbProvider = new ModuleDbProvider();
      storageConfiguration.AddModuleDbProvider(moduleDbProvider);
    }
  }
}
