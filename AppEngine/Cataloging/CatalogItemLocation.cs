namespace Mt.MediaMan.AppEngine.Cataloging
{
  /// <summary>
  /// Identifies a location of a FS catalog item.
  /// Because we don't keep directories in the catalog, in order to locate an item, we need the
  /// scan root and the path prefix.
  /// </summary>
  public class CatalogItemLocation
  {
    /// <summary>
    /// Ctor.
    /// </summary>
    public CatalogItemLocation(int scanRootId, string pathPrefix)
    {
      this.ScanRootId = scanRootId;
      this.PathPrefix = pathPrefix;
    }

    /// <summary>
    /// The scan root item id.
    /// </summary>
    public int ScanRootId { get; }

    /// <summary>
    /// The full path prefix.
    /// </summary>
    public string PathPrefix { get; }
  }
}
