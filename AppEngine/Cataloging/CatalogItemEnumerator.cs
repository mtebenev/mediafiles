using System.Collections.Generic;
using System.Linq;

namespace Mt.MediaFiles.AppEngine.Cataloging
{
  internal class CatalogItemEnumerator
  {
    private readonly ICatalog _catalog;
    private readonly Queue<ICatalogItem> _itemQueue;
    private int _startItemId;

    /// <summary>
    /// Ctor.
    /// </summary>
    public CatalogItemEnumerator(ICatalog catalog, int rootCatalogItemId)
    {
      this._catalog = catalog;
      this._itemQueue = new Queue<ICatalogItem>();
      this._startItemId = rootCatalogItemId;
    }

    public async IAsyncEnumerable<ICatalogItem> EnumerateAsync()
    {
      var rootCatalogItem = await this._catalog.GetItemByIdAsync(this._startItemId);
      this._itemQueue.Enqueue(rootCatalogItem);

      while(this._itemQueue.Count > 0)
      {
        var item = this._itemQueue.Dequeue();
        var children = await item.GetChildrenAsync();

        children
          .ToList()
          .ForEach(c => { this._itemQueue.Enqueue(c); });

        yield return item;
      }
    }
  }

}
