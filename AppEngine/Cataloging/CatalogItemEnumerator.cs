using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mt.MediaMan.AppEngine.Cataloging
{
  internal class CatalogItemEnumerator : IAsyncEnumerator<ICatalogItem>
  {
    private readonly Catalog _catalog;
    private readonly Func<ICatalogItem, Task> _processFunc;
    private readonly Queue<int> _idQueue;
    private ICatalogItem _currentCatalogItem;

    /// <summary>
    /// processFunc will be awaited after the item has been retrieved
    /// </summary>
    public CatalogItemEnumerator(Catalog catalog, int rootCatalogItemId, Func<ICatalogItem, Task> processFunc)
    {
      _catalog = catalog;
      _processFunc = processFunc;
      _idQueue = new Queue<int>();

      _idQueue.Enqueue(rootCatalogItemId);
    }

    public void Dispose()
    {
    }

    public async Task<bool> MoveNext(CancellationToken cancellationToken)
    {
      if(_idQueue.Count == 0)
        return false;
      else
      {
        var itemId = _idQueue.Dequeue();
        _currentCatalogItem = await _catalog.GetItemByIdAsync(itemId);
        var children = await _currentCatalogItem.GetChildrenAsync();
        children
          .Select(c => c.CatalogItemId)
          .ToList()
          .ForEach(id => { _idQueue.Enqueue(id); });

        // Execute an action during for this iteration
        await _processFunc(_currentCatalogItem);

        return _idQueue.Count > 0;
      }
    }

    public ICatalogItem Current => _currentCatalogItem;
  }

}
