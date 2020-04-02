using Mt.MediaMan.AppEngine.CatalogStorage;

namespace Mt.MediaMan.AppEngine.Cataloging
{
  /// <summary>
  /// Instantiates a catalog item.
  /// </summary>
  internal interface ICatalogItemFactory
  {
    /// <summary>
    /// Creates a child item object.
    /// </summary>
    ICatalogItem CreateItem(CatalogItemRecord record);
  }
}
