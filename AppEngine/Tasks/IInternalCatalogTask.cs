using System.Threading.Tasks;
using Mt.MediaFiles.AppEngine.Cataloging;

namespace Mt.MediaFiles.AppEngine.Tasks
{
  /// <summary>
  /// Internal commands (implemented in the AppEngine) should implement this interface.
  /// These commands have access to some catalog internal facilities.
  /// </summary>
  internal interface IInternalCatalogTask
  {
    public Task ExecuteAsync(ICatalog catalog)
    {
      return catalog.ExecuteTaskAsync(this);
    }

    internal Task ExecuteAsync(Catalog catalog);
  }

  /// <summary>
  /// The internal command with result.
  /// </summary>
  internal interface IInternalCatalogTask<TResult>
  {
    public Task<TResult> ExecuteAsync(ICatalog catalog)
    {
      return catalog.ExecuteTaskAsync(this);
    }

    internal Task<TResult> ExecuteAsync(Catalog catalog);
  }
}
