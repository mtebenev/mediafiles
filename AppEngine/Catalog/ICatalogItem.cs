using System.Collections.Generic;

namespace Mt.MediaMan.AppEngine.Catalog
{
  public interface ICatalogItem
  {
    int CatalogItemId { get; }
    
    /// <summary>
    /// Usually corresponds to a file name
    /// </summary>
    string Name { get; }

    // Navigation
    ICatalogItem ParentItem { get; }
    IEnumerable<ICatalogItem> Children { get; }
  }
}