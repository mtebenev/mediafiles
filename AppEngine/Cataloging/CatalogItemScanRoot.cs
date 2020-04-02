using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.CatalogStorage;
using Mt.MediaMan.AppEngine.Scanning;

namespace Mt.MediaMan.AppEngine.Cataloging
{
  /// <summary>
  /// Implementation for scan root items.
  /// </summary>
  internal class CatalogItemScanRoot : CatalogItemBase
  {
    private readonly IStructureAccessFactory _structureAccessFactory;

    public CatalogItemScanRoot(CatalogItemRecord catalogItemRecord, IItemStorage itemStorage, ICatalogItemFactory catalogItemFactory, IStructureAccessFactory structureAccessFactory)
      : base(catalogItemRecord, itemStorage, catalogItemFactory)
    {
      this._structureAccessFactory = structureAccessFactory;
    }

    /// <summary>
    /// ICatalogItem.
    /// </summary>
    public override bool IsDirectory => true;

    /// <summary>
    /// ICatalogItem.
    /// </summary>
    public override async Task<IList<ICatalogItem>> GetChildrenAsync()
    {
      var structureAccess = await this._structureAccessFactory.CreateAsync(this.CatalogItemId);
      var infoPartScanRoot = await this.GetInfoPartAsync<InfoPartScanRoot>();
      var location = new CatalogItemLocation(this.CatalogItemId, infoPartScanRoot.RootPath);

      var records = await structureAccess.QueryLevelAsync(location);

      var result = records
        .Select(r => r.ItemType == CatalogItemType.VirtualFolder
        ? this.CreateVirtualFolderItem(r)
        : this.CreateChildItem(r))
        .ToList();

      return result;
    }

    /// <summary>
    /// Creates a new virtual folder item based on the record.
    /// </summary>
    private ICatalogItem CreateVirtualFolderItem(CatalogItemRecord record)
    {
      var result = new CatalogItemVirtualFolder(record, this._itemStorage, this._catalogItemFactory);
      return result;
    }
  }
}
