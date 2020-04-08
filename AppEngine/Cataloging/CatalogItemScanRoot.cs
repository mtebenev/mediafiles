using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using Mt.MediaFiles.AppEngine.Scanning;

namespace Mt.MediaFiles.AppEngine.Cataloging
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
    public override string Name => this.Path;

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
        ? this.CreateVirtualFolderItem(r, structureAccess, location)
        : this.CreateChildItem(r))
        .ToList();

      return result;
    }

    /// <summary>
    /// Creates a new virtual folder item based on the record.
    /// </summary>
    private ICatalogItem CreateVirtualFolderItem(CatalogItemRecord record, IStructureAccess structureAccess, CatalogItemLocation location)
    {
      var childLocation = location.CreateChildLocation(record.Path);
      var result = new CatalogItemVirtualFolder(record, this._itemStorage, this._catalogItemFactory, childLocation, structureAccess);
      return result;
    }
  }
}
