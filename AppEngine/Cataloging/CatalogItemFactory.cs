using Mt.MediaFiles.AppEngine.CatalogStorage;

namespace Mt.MediaFiles.AppEngine.Cataloging
{
  /// <summary>
  /// Encapsulates logic for creating catalog items.
  /// </summary>
  internal class CatalogItemFactory : ICatalogItemFactory
  {
    private readonly IItemStorage _itemStorage;
    private readonly IStructureAccessFactory _structureAccessFactory;

    public CatalogItemFactory(IItemStorage itemStorage, IStructureAccessFactory structureAccessFactory)
    {
      this._itemStorage = itemStorage;
      this._structureAccessFactory = structureAccessFactory;
    }

    /// <summary>
    /// Creates new catalog item from the child record.
    /// </summary>
    public ICatalogItem CreateItem(CatalogItemRecord record)
    {
      ICatalogItem result;
      switch(record.ItemType)
      {
        case CatalogItemType.ScanRoot:
          result = new CatalogItemScanRoot(record, this._itemStorage, this, this._structureAccessFactory);
          break;
        default:
          result = new CatalogItem(record, this._itemStorage, this);
          break;
      }

      return result;
    }
  }
}
