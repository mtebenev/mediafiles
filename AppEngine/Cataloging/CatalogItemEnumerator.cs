using System.Collections.Generic;
using System.Linq;

namespace Mt.MediaMan.AppEngine.Cataloging
{
  internal class CatalogItemEnumerator
  {
    private readonly ICatalog _catalog;
    private readonly Queue<int> _idQueue;
    private ICatalogItem _currentCatalogItem;

    /// <summary>
    /// processFunc will be awaited after the item has been retrieved
    /// </summary>
    public CatalogItemEnumerator(ICatalog catalog, int rootCatalogItemId)
    {
      this._catalog = catalog;
      this._idQueue = new Queue<int>();

      this._idQueue.Enqueue(rootCatalogItemId);
    }

    // TODOA remove?
    public ICatalogItem Current => _currentCatalogItem;

    public async IAsyncEnumerable<ICatalogItem> WalkAsync()
    {
      while(_idQueue.Count > 0)
      {
        var itemId = _idQueue.Dequeue();
        this._currentCatalogItem = await this._catalog.GetItemByIdAsync(itemId);
        var children = await this._currentCatalogItem.GetChildrenAsync();
        children
          .Select(c => c.CatalogItemId)
          .ToList()
          .ForEach(id => { _idQueue.Enqueue(id); });

        yield return this._currentCatalogItem;
      }
    }
  }

}
