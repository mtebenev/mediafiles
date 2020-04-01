using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.CatalogStorage;

namespace Mt.MediaMan.AppEngine.Scanning
{
  /// <summary>
  /// The item explorer for file system.
  /// </summary>
  internal class ItemExplorerFileSystem : IItemExplorer
  {
    private readonly IFileSystem _fileSystem;

    public ItemExplorerFileSystem(IFileSystem fileSystem)
    {
      this._fileSystem = fileSystem;
    }

    public IAsyncEnumerable<CatalogItemRecord> Explore(string scanPath, int scanRootId)
    {
      var directoryInfo = this._fileSystem.DirectoryInfo.FromDirectoryName(scanPath);

      var result = directoryInfo
        .EnumerateFiles("*", SearchOption.AllDirectories)
        .Select(x => this.CreateRecord(x, scanRootId))
        .ToAsyncEnumerable();

      return result;
    }

    private CatalogItemRecord CreateRecord(IFileInfo fileInfo, int parentItemId)
    {
      var record = new CatalogItemRecord
      {
        Path = fileInfo.FullName,
        Size = fileInfo.Length,
        ParentItemId = parentItemId,
        ItemType = CatalogItemType.File
      };

      return record;
    }
  }
}
