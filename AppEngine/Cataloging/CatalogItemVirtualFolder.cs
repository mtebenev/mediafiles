using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.CatalogStorage;

namespace Mt.MediaMan.AppEngine.Cataloging
{
  /// <summary>
  /// The virtual folder item.
  /// Design note: we don't keep the FS directories in the catalog. This item acts as a virtual one, representing
  /// the physical directory.
  /// </summary>
  internal class CatalogItemVirtualFolder : CatalogItemBase
  {
    private readonly CatalogItemLocation _location;
    private readonly IStructureAccess _structureAccess;

    /// <summary>
    /// Ctor.
    /// </summary>
    public CatalogItemVirtualFolder(
      CatalogItemRecord catalogItemRecord,
      IItemStorage itemStorage,
      ICatalogItemFactory catalogItemFactory,
      CatalogItemLocation location,
      IStructureAccess structureAccess
      )
      : base(catalogItemRecord, itemStorage, catalogItemFactory)
    {
      this._location = location;
      this._structureAccess = structureAccess;
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
      var records = await this._structureAccess.QueryLevelAsync(this._location);

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
      var childLocation = this._location.CreateChildLocation(record.Path);
      var result = new CatalogItemVirtualFolder(
        record,
        this._itemStorage,
        this._catalogItemFactory,
        childLocation,
        this._structureAccess);

      return result;
    }

  }
}
