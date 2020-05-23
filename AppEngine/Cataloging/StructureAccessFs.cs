using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using Mt.MediaFiles.AppEngine.CatalogStorage;

namespace Mt.MediaFiles.AppEngine.Cataloging
{
  /// <summary>
  /// The structure access for file system.
  /// </summary>
  internal class StructureAccessFs : IStructureAccess
  {
    private readonly IFileSystem _fileSystem;
    private readonly IItemStorage _itemStorage;
    private readonly int _scanRootId;

    public StructureAccessFs(IFileSystem fileSystem, IItemStorage itemStorage, int scanRootId)
    {
      this._fileSystem = fileSystem;
      this._itemStorage = itemStorage;
      this._scanRootId = scanRootId;
    }

    /// <summary>
    /// IStructureAccess.
    /// </summary>
    public async Task<IList<CatalogItemRecord>> QueryLevelAsync(CatalogItemLocation catalogItemLocation)
    {
      var subtreeRecords = await this._itemStorage.QuerySubtree(catalogItemLocation);

      // Find this level records
      var locationLevel = catalogItemLocation
        .PathPrefix
        .ToCharArray()
        .Count(x => x == '\\');

      var levelRecords = subtreeRecords
        .Where(x => x.Path.Count(c => c == '\\') == locationLevel + 1)
        .ToList();

      // Find records with the next level directories
      var directoryRecords = subtreeRecords
        .Where(x => x.Path.Count(c => c == '\\') > locationLevel + 1)
        .GroupBy(r =>
        {
          var rest = r.Path.Substring(catalogItemLocation.PathPrefix.Length + 1);
          var nextLevelPos = rest.IndexOf('\\');
          var key = rest.Substring(0, nextLevelPos);

          return key;
        })
        .Select(g => new CatalogItemRecord
        {
          ItemType = CatalogItemType.VirtualFolder,
          Path = g.Key
        })
        .ToList();

      var result = levelRecords;
      result.AddRange(directoryRecords);
      return result;
    }
  }
}
