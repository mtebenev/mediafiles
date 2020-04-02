using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.CatalogStorage;

namespace Mt.MediaMan.AppEngine.Cataloging
{
  /// <summary>
  /// Common catalog item implementation.
  /// </summary>
  internal class CatalogItem : CatalogItemBase
  {
    public CatalogItem(CatalogItemRecord catalogItemRecord, IItemStorage itemStorage, ICatalogItemFactory catalogItemFactory)
      : base(catalogItemRecord, itemStorage, catalogItemFactory)
    {
    }

    /// <summary>
    /// ICatalogItem.
    /// </summary>
    public override async Task<IList<ICatalogItem>> GetChildrenAsync()
    {
      var childrenItemRecords = await this._itemStorage.LoadChildrenAsync(_catalogItemRecord.CatalogItemId);
      var childrenItems = childrenItemRecords
        .Select(r => this.CreateChildItem(r))
        .ToList();

      return childrenItems;
    }

    /// <summary>
    /// ICatalogItem
    /// </summary>
    public override bool IsDirectory => this.Size == -1;
  }
}
