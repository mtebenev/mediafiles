using System.Collections.Generic;
using Mt.MediaMan.AppEngine.CatalogStorage;

namespace Mt.MediaMan.AppEngine.Cataloging
{
  /// <summary>
  /// Startup storage configuration for catalog storage
  /// </summary>
  public class StorageConfiguration
  {
    private readonly List<IModuleStorageProvider> _moduleStorageProviders;

    public StorageConfiguration()
    {
      _moduleStorageProviders = new List<IModuleStorageProvider>();
    }

    /// <summary>
    /// Registered module storage providers
    /// </summary>
    public IReadOnlyList<IModuleStorageProvider> ModuleStorageProviders => _moduleStorageProviders;

    public void AddModuleStorageProvider(IModuleStorageProvider moduleStorageProvider)
    {
      _moduleStorageProviders.Add(moduleStorageProvider);
    }
  }
}
