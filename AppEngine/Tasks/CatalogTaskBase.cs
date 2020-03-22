using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Cataloging;

namespace Mt.MediaMan.AppEngine.Tasks
{
  /// <summary>
  /// Common catalog tasks should inherit this class.
  /// </summary>
  public abstract class CatalogTaskBase
  {
    internal Task ExecuteAsync(Catalog catalog)
    {
      var context = new CatalogContext(catalog);
      return this.ExecuteAsync(context);
    }

    internal Task ExecuteTaskAsync(ICatalogContext catalogContext)
    {
      return this.ExecuteAsync(catalogContext);
    }

    /// <summary>
    /// Override and implement the task logic in this method.
    /// </summary>
    protected abstract Task ExecuteAsync(ICatalogContext catalogContext);
  }

  /// <summary>
  /// Common catalog tasks should inherit this class.
  /// </summary>
  public abstract class CatalogTaskBase<TResult>
  {
    internal Task<TResult> ExecuteAsync(Catalog catalog)
    {
      var context = new CatalogContext(catalog);
      return this.ExecuteTaskAsync(context);
    }

    internal Task<TResult> ExecuteTaskAsync(ICatalogContext catalogContext)
    {
      return this.ExecuteAsync(catalogContext);
    }

    /// <summary>
    /// Override and implement the task logic in this method.
    /// </summary>
    protected abstract Task<TResult> ExecuteAsync(ICatalogContext catalogContext);
  }
}
