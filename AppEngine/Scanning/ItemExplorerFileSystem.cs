using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using MoreLinq;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.CatalogStorage;

namespace Mt.MediaFiles.AppEngine.Scanning
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

    /// <summary>
    /// IItemExplorer.
    /// </summary>
    public Task<InfoPartScanRoot> CreateScanRootPartAsync(string scanPath)
    {
      var directoryInfo = this._fileSystem.DirectoryInfo.FromDirectoryName(scanPath);
      var filePathRoot = this._fileSystem.Path.GetPathRoot(scanPath);
      var infoPartScanRoot = new InfoPartScanRoot();

      // Check for UNC
      var pathUri = new Uri(filePathRoot);
      if(pathUri.IsUnc)
        infoPartScanRoot.DriveType = DriveType.Network.ToString();
      else
      {
        var drives = this._fileSystem.DriveInfo.GetDrives();
        var driveInfo = drives.First(di => di.Name.Equals(filePathRoot, StringComparison.InvariantCultureIgnoreCase));
        infoPartScanRoot.DriveType = driveInfo.DriveType.ToString();
      }

      infoPartScanRoot.RootPath = scanPath;
      return Task.FromResult(infoPartScanRoot);
    }

    /// <summary>
    /// IItemExplorer.
    /// </summary>
    public IAsyncEnumerable<CatalogItemRecord> Explore(string scanPath, int scanRootId, IScanConfiguration scanConfiguration)
    {
      var directoryInfo = this._fileSystem.DirectoryInfo.FromDirectoryName(scanPath);

      var topLevel = directoryInfo
        .EnumerateFileSystemInfos("*", SearchOption.TopDirectoryOnly)
        .Where(fsi => !scanConfiguration.IsIgnoredEntry(fsi.Name))
        .ToList();

      // Enumerate top-level files
      var topLevelFiles = topLevel
        .Where(fsi => !fsi.Attributes.HasFlag(FileAttributes.Directory))
        .Where(fsi => !scanConfiguration.IsIgnoredEntry(fsi.Name))
        .Select(fsi => this._fileSystem.FileInfo.FromFileName(fsi.FullName))
        .Select(x => this.CreateRecord(x, scanRootId));

      var nestedFiles = topLevel
        .Where(fsi => fsi.Attributes.HasFlag(FileAttributes.Directory))
        .Where(fsi => !scanConfiguration.IsIgnoredEntry(fsi.Name))
        .Select(fsi => this._fileSystem.DirectoryInfo.FromDirectoryName(fsi.FullName))
        .SelectMany(fsi => fsi.EnumerateFiles("*", SearchOption.AllDirectories))
        .Select(x => this.CreateRecord(x, scanRootId));

      var result = topLevelFiles
        .Concat(nestedFiles)
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
