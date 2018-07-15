using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mt.MediaMan.AppEngine.Cataloging
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
    public static IAsyncEnumerable<ICatalogItem> CreateDefaultWalker(Catalog catalog, int catalogItemId)
    {
      var enumerable = AsyncEnumerable.CreateEnumerable(() => new CatalogItemEnumerator(catalog, catalogItemId, item => Task.CompletedTask));
      return enumerable;
    }

    /// <summary>
    /// Walker with async action executed after the next item sucessesfully retrieved
    /// Note: this is because Ix.ForEachAsync does not actually awaits for action execution
    /// https://github.com/dotnet/reactive/issues/144 for more details
    /// </summary>
    public static IAsyncEnumerable<ICatalogItem> CreateDefaultWalker(Catalog catalog, int catalogItemId, Func<ICatalogItem, Task> processFunc)
    {
      var enumerable = AsyncEnumerable.CreateEnumerable(() => new CatalogItemEnumerator(catalog, catalogItemId, processFunc));
      return enumerable;
    }
  }
}
