using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.CatalogStorage;
using Mt.MediaMan.AppEngine.Scanning;

namespace Mt.MediaMan.AppEngine.Cataloging
{
  internal abstract class CatalogItemBase : ICatalogItem
  {
    protected readonly CatalogItemRecord _catalogItemRecord;
    protected CatalogItemData _catalogItemData;
    protected readonly IItemStorage _itemStorage;
    protected readonly ICatalogItemFactory _catalogItemFactory;

    public CatalogItemBase(CatalogItemRecord catalogItemRecord, IItemStorage itemStorage, ICatalogItemFactory catalogItemFactory)
    {
      this._catalogItemRecord = catalogItemRecord;
      this._itemStorage = itemStorage;
      this._catalogItemFactory = catalogItemFactory;
      this._catalogItemData = null;
    }

    /// <summary>
    /// ICatalogItem.
    /// </summary>
    public int CatalogItemId => this._catalogItemRecord.CatalogItemId;

    /// <summary>
    /// ICatalogItem.
    /// </summary>
    public string Path => this._catalogItemRecord.Path;

    /// <summary>
    /// ICatalogItem.
    /// </summary>
    public long Size => this._catalogItemRecord.Size;

    /// <summary>
    /// ICatalogItem.
    /// </summary>
    public string ItemType => this._catalogItemRecord.ItemType;

    /// <summary>
    /// ICatalogItem.
    /// </summary>
    public async Task<TInfoPart> GetInfoPartAsync<TInfoPart>() where TInfoPart : InfoPartBase
    {
      await EnsureItemDataLoaded();

      var result = _catalogItemData.Get<TInfoPart>();
      return result;
    }

    /// <summary>
    /// ICatalogItem.
    /// </summary>
    public abstract Task<IList<ICatalogItem>> GetChildrenAsync();

    /// <summary>
    /// ICatalogItem.
    /// </summary>
    public async Task<IList<string>> GetInfoPartNamesAsync()
    {
      await EnsureItemDataLoaded();

      var result = _catalogItemData.Data.Properties().Select(p => p.Name).ToList();
      return result;
    }

    public abstract string Name { get; }
    public abstract bool IsDirectory { get; }

    private async Task EnsureItemDataLoaded()
    {
      if(_catalogItemData == null)
        _catalogItemData = await _itemStorage.LoadItemDataAsync(_catalogItemRecord.CatalogItemId);
    }

    /// <summary>
    /// Common factory methods for creating related items.
    /// </summary>
    protected ICatalogItem CreateChildItem(CatalogItemRecord record)
    {
      var result = this._catalogItemFactory.CreateItem(record);
      return result;
    }
  }
}
