using System;
using System.Threading.Tasks;

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
    Task ExecuteTaskAsync(ICatalogTask catalogTask);

    /// <summary>
    /// Closes the catalog.
    /// </summary>
    void Close();
  }
}
