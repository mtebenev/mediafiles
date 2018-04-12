namespace Mt.MediaMan.AppEngine.Scanning
{
  /// <summary>
  /// Abstract factory for file handlers
  /// </summary>
  internal interface IFileHandlerFactory
  {
    /// <summary>
    /// Unique ID of the handler
    /// </summary>
    string Id { get; }

    IScanDriver ScanDriver { get; }
    ICatalogItemIndexer CatalogItemIndexer { get; }

  }
}
