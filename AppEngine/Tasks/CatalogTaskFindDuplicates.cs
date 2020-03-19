using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Tools;

namespace Mt.MediaMan.AppEngine.Tasks
{
  /// <summary>
  /// Finds file duplicates.
  /// </summary>
  public class CatalogTaskFindDuplicates : CatalogTaskBase<IList<DuplicateFindResult>>
  {
    /// <summary>
    /// ICatalogTask
    /// </summary>
    public override async Task<IList<DuplicateFindResult>> ExecuteAsync(ICatalogContext catalogContext)
    {
      var itemHashes = await this.CollectHashesAsync(catalogContext.Catalog);
      var candidates = this.CollectCandidates(itemHashes);

      var result = await CreateResultAsync(catalogContext.Catalog, candidates);
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

    private async Task<IList<DuplicateFindResult>> CreateResultAsync(ICatalog catalog, List<List<ItemHashInfo>> candidates)
    {
      var result = new List<DuplicateFindResult>();

      foreach(var candidate in candidates)
      {
        var itemIds = candidate.Select(i => i.CatalogItemId).ToList();
        var duplicateFindResult = await DuplicateFindResult.CreateAsync(catalog, itemIds);

        result.Add(duplicateFindResult);
      }

      return result;
    }

    private async Task<List<ItemHashInfo>> CollectHashesAsync(ICatalog catalog)
    {
      int catalogItemId = catalog.RootItem.CatalogItemId; // Start from catalog root
      var walker = CatalogTreeWalker.CreateDefaultWalker(catalog, catalogItemId);
      var result = await walker
        .Select(ci => this.CreateItemHash(ci))
        .ToListAsync();

      return result;
    }

    private ItemHashInfo CreateItemHash(ICatalogItem item)
    {
      var hash = new
      {
        item.Name,
        item.Size
      }.GetHashCode();

      var itemHashInfo = new ItemHashInfo(item.CatalogItemId, hash);
      return itemHashInfo;
    }
  }
}
