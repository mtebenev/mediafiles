using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.CatalogStorage;
using Mt.MediaMan.AppEngine.Scanning;

namespace Mt.MediaMan.AppEngine.Cataloging
{
  internal class CatalogItem : ICatalogItem
  {
    private readonly CatalogItemRecord _catalogItemRecord;
    private CatalogItemData _catalogItemData;
    private readonly IItemStorage _itemStorage;

    public CatalogItem(CatalogItemRecord catalogItemRecord, IItemStorage itemStorage)
    {
      _catalogItemRecord = catalogItemRecord;
      _itemStorage = itemStorage;
      _catalogItemData = null;
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
    public long Size => _catalogItemRecord.Size;

    /// <summary>
    /// ICatalogItem
    /// </summary>
    public bool IsDirectory => this.Size == -1;

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

    /// <summary>
    /// ICatalogItem
    /// </summary>
    public async Task<TInfoPart> GetInfoPartAsync<TInfoPart>() where TInfoPart : InfoPartBase
    {
      await EnsureItemDataLoaded();

      var result = _catalogItemData.Get<TInfoPart>();
      return result;
    }

    public async Task<IList<string>> GetInfoPartNamesAsync()
    {
      await EnsureItemDataLoaded();

      var result = _catalogItemData.Data.Properties().Select(p => p.Name).ToList();
      return result;
    }

    private async Task EnsureItemDataLoaded()
    {
      if(_catalogItemData == null)
        _catalogItemData = await _itemStorage.LoadItemDataAsync(_catalogItemRecord.CatalogItemId);
    }
  }
}
