using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.Ebooks.Storage;

namespace Mt.MediaFiles.AppEngine.Ebooks
{
  public static class EbooksModule
  {
    public static void CreateStorageConfiguration(StorageConfiguration storageConfiguration)
    {
      var storageProvider = new ModuleStorageProvider();
      storageConfiguration.AddModuleStorageProvider(storageProvider);
    }
  }
}
