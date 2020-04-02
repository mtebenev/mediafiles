using System;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Tasks;

namespace Mt.MediaMan.AppEngine.Cataloging
{
  /// <summary>
  /// Basic catalog interface
  /// </summary>
  public interface ICatalog : IDisposable
  {
    /// <summary>
    /// The catalog name.
    /// </summary>
    string CatalogName { get; }

    /// <summary>
    /// The root catalog item.
    /// </summary>
    ICatalogItem RootItem { get; }

    /// <summary>
    /// Loads an item with specified ID
    /// </summary>
    Task<ICatalogItem> GetItemByIdAsync(int itemId);

    /// <summary>
    /// Executes the catalog task.
    /// </summary>
    Task ExecuteTaskAsync(CatalogTaskBase catalogTask);

    /// <summary>
    /// Executes the catalog task with result.
    /// </summary>
    Task<TResult> ExecuteTaskAsync<TResult>(CatalogTaskBase<TResult> task);

    /// <summary>
    /// An internal task execution.
    /// </summary>
    internal Task ExecuteTaskAsync(IInternalCatalogTask task);

    /// <summary>
    /// An internal task execution with result.
    /// </summary>
    internal Task<TResult> ExecuteTaskAsync<TResult>(IInternalCatalogTask<TResult> task);

    /// <summary>
    /// Closes the catalog.
    /// </summary>
    void Close();
  }
}
