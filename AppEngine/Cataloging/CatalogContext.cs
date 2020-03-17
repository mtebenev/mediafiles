namespace Mt.MediaMan.AppEngine.Cataloging
{
  /// <summary>
  /// The catalog context implementation.
  /// </summary>
  internal class CatalogContext : ICatalogContext
  {
    private readonly Catalog _catalog;

    /// <summary>
    /// Ctor.
    /// </summary>
    public CatalogContext(Catalog catalog)
    {
      this._catalog = catalog;
    }

    /// <summary>
    /// ICatalogContext.
    /// </summary>
    public ICatalog Catalog => this._catalog;
  }
}
