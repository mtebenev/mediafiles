using Mt.MediaFiles.AppEngine.CatalogStorage;

namespace Mt.MediaFiles.AppEngine.Cataloging
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
