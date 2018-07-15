using System.Threading.Tasks;
using YesSql;
using YesSql.Sql;

namespace Mt.MediaMan.AppEngine.CatalogStorage
{
  /// <summary>
  /// Modules should provide the instance to let initialize module-specific storage stuff
  /// </summary>
  public interface IModuleStorageProvider
  {
    /// <summary>
    /// Invoked by engine when catalog storage gets initialized first time
    /// </summary>
    Task InitializeStoreAsync(ISession session, SchemaBuilder schemaBuilder);
  }
}
