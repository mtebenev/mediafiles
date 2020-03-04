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
    private readonly List<IModuleDbProvider> _moduleDbProviders;

    public StorageConfiguration()
    {
      this._moduleStorageProviders = new List<IModuleStorageProvider>();
      this._moduleDbProviders = new List<IModuleDbProvider>();
    }

    /// <summary>
    /// Registered module storage providers.
    /// </summary>
    public IReadOnlyList<IModuleStorageProvider> ModuleStorageProviders => this._moduleStorageProviders;

    /// <summary>
    /// Registered module db providers.
    /// </summary>
    public IReadOnlyList<IModuleDbProvider> ModuleDbProviders => this._moduleDbProviders;

    /// <summary>
    /// Adds a document storage provider.
    /// </summary>
    public void AddModuleStorageProvider(IModuleStorageProvider moduleStorageProvider)
    {
      _moduleStorageProviders.Add(moduleStorageProvider);
    }

    /// <summary>
    /// Adds a db storage provider.
    /// </summary>
    public void AddModuleDbProvider(IModuleDbProvider moduleDbProvider)
    {
      this._moduleDbProviders.Add(moduleDbProvider);
    }
  }
}
