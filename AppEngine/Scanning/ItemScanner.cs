using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.CatalogStorage;

namespace Mt.MediaMan.AppEngine.Scanning
{
  public interface IItemScannerFactory
  {
    internal IItemScanner Create(IItemExplorer itemExplorer, int parentItemId, string scanPath);
  }

  /// <summary>
  /// The new item scanner.
  /// </summary>
  internal class ItemScanner : IItemScanner
  {
    private readonly int _parentItemId;
    private readonly IItemExplorer _itemExplorer;
    private readonly string _scanPath;
    private readonly ILogger<ItemScanner> _logger;

    /// <summary>
    /// Ctor.
    /// </summary>
    public ItemScanner(ILoggerFactory loggerFactory, IItemExplorer itemExplorer, int parentItemId, string scanPath)
    {
      this._parentItemId = parentItemId;
      this._itemExplorer = itemExplorer;
      this._scanPath = scanPath;
      this._logger = loggerFactory.CreateLogger<ItemScanner>();
    }

    /// <summary>
    /// IItemScanner.
    /// </summary>
    public async Task Scan(IScanContext scanContext)
    {
      this._logger.LogInformation("Scanning started");

      var scanRootItemId = await this.CreateScanRootItemAsync(scanContext.ItemStorage, scanContext.ScanConfiguration.ScanRootItemName);

      var records = await this._itemExplorer.Explore(this._scanPath, scanRootItemId)
        .ToListAsync();

      foreach(var r in records)
      {
        await scanContext.ItemStorage.CreateItemAsync(r);
      }

      _logger.LogInformation("Scanning finished");
    }

    /// <summary>
    /// Stores catalog item data for scan root
    /// </summary>
    private async Task<int> CreateScanRootItemAsync(IItemStorage itemStorage, string itemName)
    {
      // Create the store record
      var scanRootRecord = new CatalogItemRecord
      {
        Path = String.IsNullOrWhiteSpace(itemName)
        ? "[SCAN_ROOT]"
        : itemName,
        Size = 0,
        ParentItemId = this._parentItemId,
        ItemType = CatalogItemType.ScanRoot
      };
      var scanRootItemId = await itemStorage.CreateItemAsync(scanRootRecord);

      // Info part
      var catalogItemData = new CatalogItemData(scanRootItemId);
      var infoPartScanRoot = await this._itemExplorer.CreateScanRootPartAsync(this._scanPath);
      catalogItemData.Apply(infoPartScanRoot);

      await itemStorage.SaveItemDataAsync(scanRootItemId, catalogItemData);

      return scanRootItemId;
    }
  }
}
