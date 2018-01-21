using System.Collections.Generic;

namespace Mt.MediaMan.AppEngine.Catalog
{
  public class CatalogItem : ICatalogItem
  {
    private readonly ICatalogItem _parentItem;
    private readonly ICatalogItem[] _childrenItems;

    public CatalogItem()
    {
      _parentItem = null;
      _childrenItems = new ICatalogItem[]{};
    }

    public int CatalogItemId { get; private set; }
    public string Name { get; set; }
    
    public ICatalogItem ParentItem => _parentItem;
    public IEnumerable<ICatalogItem> Children => _childrenItems;
  }
}