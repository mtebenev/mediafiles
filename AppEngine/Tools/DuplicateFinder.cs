using Mt.MediaMan.AppEngine.Cataloging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mt.MediaMan.AppEngine.Tools
{
  /// <summary>
  /// Searches for duplicate items in catalog
  /// </summary>
  public class DuplicateFinder
  {
    private readonly Catalog _catalog;

    public DuplicateFinder(Catalog catalog)
    {
      _catalog = catalog;
    }

    public async Task<IList<DuplicateFindResult>> FindAsync()
    {
      var itemHashes = await CollectHashesAsync();
      var candidates = CollectCandidates(itemHashes);

      var result = CreateResult(candidates);
      return result;
    }

    private List<List<ItemHashInfo>> CollectCandidates(List<ItemHashInfo> itemHashes)
    {
      var result = itemHashes
        .GroupBy(ih => ih.Hash)
        .Where(g => g.Count() > 1)
        .Select(g => g.ToList())
        .ToList();

      return result;
    }

    private IList<DuplicateFindResult> CreateResult(List<List<ItemHashInfo>> candidates)
    {
      var result = candidates.Select(c =>
      {
        var itemIds = c.Select(i => i.CatalogItemId).ToList();
        var duplicateFindResult = new DuplicateFindResult(itemIds);

        return duplicateFindResult;
      }).ToList();

      return result;
    }

    private async Task<List<ItemHashInfo>> CollectHashesAsync()
    {
      var result = new List<ItemHashInfo>();

      int catalogItemId = _catalog.RootItem.CatalogItemId; // Start from catalog root
      var walker = CatalogTreeWalker.CreateDefaultWalker(_catalog, catalogItemId, item => CreateItemHash(item, result));

      await walker.ForEachAsync(item => {});
      return result;
    }

    private Task CreateItemHash(ICatalogItem item, List<ItemHashInfo> hashCollection)
    {
      var hash = new
      {
        item.Name,
        item.Size
      }.GetHashCode();

      var itemHashInfo = new ItemHashInfo(item.CatalogItemId, hash);
      hashCollection.Add(itemHashInfo);
      return Task.CompletedTask;
    }
  }
}
