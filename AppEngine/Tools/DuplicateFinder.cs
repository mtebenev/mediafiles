using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mt.MediaFiles.AppEngine.Tools
{
  /// <summary>
  /// Searches for duplicate items in catalog
  /// </summary>
  public class DuplicateFinder
  {
    private readonly ICatalog _catalog;

    public DuplicateFinder(ICatalog catalog)
    {
      _catalog = catalog;
    }

    public async Task<IList<DuplicateFindResult>> FindAsync()
    {
      var itemHashes = await this.CollectHashesAsync();
      var candidates = this.CollectCandidates(itemHashes);

      var result = await CreateResultAsync(candidates);
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

    private async Task<IList<DuplicateFindResult>> CreateResultAsync(List<List<ItemHashInfo>> candidates)
    {
      var result = new List<DuplicateFindResult>();

      foreach(var candidate in candidates)
      {
        var itemIds = candidate.Select(i => i.CatalogItemId).ToList();
        var duplicateFindResult = await DuplicateFindResult.CreateAsync(this._catalog, itemIds);

        result.Add(duplicateFindResult);
      }

      return result;
    }

    private async Task<List<ItemHashInfo>> CollectHashesAsync()
    {
      var catalogItemId = _catalog.RootItem.CatalogItemId; // Start from catalog root
      var walker = CatalogTreeWalker.CreateDefaultWalker(_catalog, catalogItemId);
      var result = await walker
        .Where(ci => ci.ItemType == CatalogItemType.File)
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
