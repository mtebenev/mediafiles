using System.Data;
using System.Threading.Tasks;

namespace Mt.MediaFiles.AppEngine.CatalogStorage
{
  /// <summary>
  /// Modules should provide the instance to let initialize module-specific DB stuff
  /// </summary>
  public interface IModuleDbProvider
  {
    /// <summary>
    /// Invoked by engine when catalog db gets initialized first time
    /// </summary>
    Task InitializeDbAsync(IDbConnection dbConnection);
  }
}
