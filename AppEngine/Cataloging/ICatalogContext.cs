namespace Mt.MediaMan.AppEngine.Cataloging
{
  /// <summary>
  /// Provides access to the core catalog facilities.
  /// </summary>
  public interface ICatalogContext
  {
    /// <summary>
    /// The catalog.
    /// </summary>
    ICatalog Catalog { get; }
  }
}
