using System.Collections.Generic;

namespace Mt.MediaFiles.AppEngine.Cataloging
{
  /// <summary>
  /// Creates various catalog walkers
  /// </summary>
  public static class CatalogTreeWalker
  {
    /// <summary>
    /// Creates default catalog tree walker.
    /// The walker enumerates all items in subtree starting from catalogItemId. The first returned item is catalogItemId
    /// </summary>
    public static IAsyncEnumerable<ICatalogItem> CreateDefaultWalker(ICatalog catalog, int catalogItemId)
    {
      var enumerator = new CatalogItemEnumerator(catalog, catalogItemId);
      return enumerator.EnumerateAsync();
    }
  }
}
