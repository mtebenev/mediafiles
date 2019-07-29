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
    /// Loads an item with specified ID
    /// </summary>
    Task<ICatalogItem> GetItemByIdAsync(int itemId);
  }
}
