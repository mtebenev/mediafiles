using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Cataloging;

namespace Mt.MediaMan.AppEngine.Tasks
{
  /// <summary>
  /// Thin interface for catalog tasks.
  /// </summary>
  public interface ICatalogTask
  {
    internal Task ExecuteAsync(Catalog catalog);
  }

  /// <summary>
  /// The task with result.
  /// </summary>
  public interface ICatalogTask<TResult>
  {
    internal Task<TResult> ExecuteAsync(Catalog catalog);
  }
}
