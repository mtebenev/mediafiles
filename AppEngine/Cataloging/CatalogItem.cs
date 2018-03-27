using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.CatalogStorage;

namespace Mt.MediaMan.AppEngine.Cataloging
{
  internal class CatalogItem : ICatalogItem
  {
    private readonly CatalogItemRecord _catalogItemRecord;
    private readonly IItemStorage _itemStorage;

    public CatalogItem(CatalogItemRecord catalogItemRecord, IItemStorage itemStorage)
    {
      _catalogItemRecord = catalogItemRecord;
      _itemStorage = itemStorage;
    }

    /// <summary>
    /// ICatalogItem
    /// </summary>
    public int CatalogItemId => _catalogItemRecord.CatalogItemId;

    /// <summary>
    /// ICatalogItem
    /// </summary>
    public string Name => _catalogItemRecord.Name;

    /// <summary>
    /// ICatalogItem
    /// </summary>
    public int Size => _catalogItemRecord.Size;

    /// <summary>
    /// ICatalogItem
    /// </summary>
    public async Task<ICatalogItem> GetParentItemAsync()
    {
      var parentItemRecord = await _itemStorage.LoadItemByIdAsync(_catalogItemRecord.ParentItemId);
      var result = new CatalogItem(parentItemRecord, _itemStorage);

      return result;
    }

    /// <summary>
    /// ICatalogItem
    /// </summary>
    public async Task<IList<ICatalogItem>> GetChildrenAsync()
    {
      var childrenItemRecords = await _itemStorage.LoadChildrenAsync(_catalogItemRecord.CatalogItemId);
      var childrenItems = childrenItemRecords
        .Select(r => new CatalogItem(r, _itemStorage))
        .Cast<ICatalogItem>()
        .ToList();

      return childrenItems;
    }
  }
}
