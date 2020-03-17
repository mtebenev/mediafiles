using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Cataloging;

namespace Mt.MediaMan.AppEngine.Tasks
{
  /// <summary>
  /// Thin interface for catalog tasks.
  /// </summary>
  public abstract class CatalogTaskBase
  {
    internal Task ExecuteAsync(Catalog catalog)
    {
      var context = new CatalogContext(catalog);
      return this.ExecuteAsync(context);
    }

    /// <summary>
    /// Override and implement the task logic in this method.
    /// </summary>
    public abstract Task ExecuteAsync(ICatalogContext catalogContext);
  }

  /// <summary>
  /// The task with result.
  /// </summary>
  public abstract class CatalogTaskBase<TResult>
  {
    internal Task<TResult> ExecuteAsync(Catalog catalog)
    {
      var context = new CatalogContext(catalog);
      return this.ExecuteAsync(context);
    }

    /// <summary>
    /// Override and implement the task logic in this method.
    /// </summary>
    public abstract Task<TResult> ExecuteAsync(ICatalogContext catalogContext);
  }
}
