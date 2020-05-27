using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MoreLinq;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using StackExchange.Profiling;

namespace Mt.MediaFiles.AppEngine.Scanning
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

      var (scanRootItemId, location) = await this.CreateScanRootItemAsync(scanContext.ItemStorage, scanContext.ScanConfiguration.ScanRootItemName);

      // Explore and save the items (file infos)
      using(var timing = MiniProfiler.Current.Step("Exploring and saving items"))
      {
        scanContext.UpdateStatus("Exploring files...");
        var records = await this._itemExplorer.Explore(this._scanPath, scanRootItemId)
          .ToListAsync();

        scanContext.UpdateStatus("Saving file records...");

        const int pageSize = 1000;
        var chunks = records.Batch(pageSize);
        foreach(var c in chunks)
        {
          await scanContext.ItemStorage.CreateManyItemsAsync(c);
        }
      }

      // Do scan sub-tasks
      using(var timing = MiniProfiler.Current.Step("Running sub-tasks"))
      {
        if(scanContext.ScanConfiguration.ScanServices.Any())
        {
          scanContext.UpdateStatus("Scanning files...");
          await this.RunScanServicesAsync(scanContext, location);
        }
      }

      scanContext.UpdateStatus("Done.");
      _logger.LogInformation("Scanning finished");
    }

    /// <summary>
    /// Stores catalog item data for scan root.
    /// Returns tuple with the scan root item id and catalog location.
    /// </summary>
    private async Task<(int, CatalogItemLocation)> CreateScanRootItemAsync(IItemStorage itemStorage, string itemName)
    {
      // Create the store record
      var scanRootRecord = new CatalogItemRecord
      {
        Path = string.IsNullOrWhiteSpace(itemName)
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
      var location = new CatalogItemLocation(scanRootItemId, infoPartScanRoot.RootPath);

      return (scanRootItemId, location);
    }

    /// <summary>
    /// Runs the scanning sub-tasks on the catalog items.
    /// </summary>
    private async Task RunScanServicesAsync(IScanContext scanContext, CatalogItemLocation location)
    {
      var scanServiceContext = new ScanServiceContext(scanContext);
      var records = await scanContext.ItemStorage.QuerySubtree(location);

      using(var progressOperation = scanContext.StartProgressOperation(records.Count))
      {
        foreach(var r in records)
        {
          scanServiceContext.SetCurrentRecord(r);
          progressOperation.Tick();
          foreach(var ss in scanContext.ScanConfiguration.ScanServices)
          {
              await this.RunSingleServiceScan(ss, scanServiceContext, r);
          }
          await scanServiceContext.SaveDataAsync(scanContext.ItemStorage);
        }

        // Finalize (flush data).
        foreach(var ss in scanContext.ScanConfiguration.ScanServices)
        {
          await ss.FlushAsync();
        }
      }
    }

    private async Task RunSingleServiceScan(IScanService scanService, ScanServiceContext scanServiceContext, CatalogItemRecord record)
    {
      try
      {
        using(var timing = MiniProfiler.Current.CustomTiming(scanService.Id, "", null, false))
        {
          await scanService.ScanAsync(scanServiceContext, record);
        }
      }
      catch(Exception e)
      {
        this._logger.LogError(e, "Scan error.");
      }
    }
  }
}
