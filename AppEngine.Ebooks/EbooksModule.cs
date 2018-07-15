using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Ebooks.Storage;

namespace Mt.MediaMan.AppEngine.Ebooks
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
