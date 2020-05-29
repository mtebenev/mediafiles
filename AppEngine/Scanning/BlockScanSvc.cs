using Microsoft.Extensions.Logging;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using Mt.MediaFiles.AppEngine.Common;
using Mt.MediaFiles.AppEngine.Tasks;
using StackExchange.Profiling;
using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Mt.MediaFiles.AppEngine.Scanning
{
  /// <summary>
  /// Pipeline block running scan services.
  /// </summary>
  internal class BlockScanSvc
  {
    private readonly ScanServiceContext _scanServiceContext;
    private readonly IScanContext _scanContext;
    private readonly IProgressOperation _progressOperation;
    private readonly ILogger _logger;

    /// <summary>
    /// Ctor.
    /// </summary>
    private BlockScanSvc(IScanContext scanContext, IProgressOperation progressOperation, ILogger logger)
    {
      this._scanServiceContext = new ScanServiceContext(scanContext);
      this._scanContext = scanContext;
      this._progressOperation = progressOperation;
      this._logger = logger;
    }

    /// <summary>
    /// Factory method.
    /// </summary>
    public static ITargetBlock<CatalogItemRecord> Create(IScanContext scanContext, IProgressOperation progressOperation, ILoggerFactory loggerFactory)
    {
      var parallelism = 4;
      var logger = loggerFactory.CreateLogger<BlockScanSvc>();
      var blockPool = new ObjectPool<BlockScanSvc>(
        () => new BlockScanSvc(scanContext, progressOperation, logger));

      var result = new ActionBlock<CatalogItemRecord>(async record =>
      {
        BlockScanSvc block = null;
        try
        {
          block = blockPool.Get();
          await block.ExecuteAsync(record);
        }
        finally
        {
          blockPool.Return(block);
        }
      },
      new ExecutionDataflowBlockOptions
      {
        MaxDegreeOfParallelism = parallelism,
        BoundedCapacity = parallelism * 2
      });

      return result;
    }

    public async Task ExecuteAsync(CatalogItemRecord record)
    {
      this._scanServiceContext.SetCurrentRecord(record);
      foreach(var ss in this._scanContext.ScanConfiguration.ScanServices)
      {
        await this.RunSingleServiceScan(ss, this._scanServiceContext, record);
      }
      await this._scanServiceContext.SaveDataAsync(this._scanContext.ItemStorage);
      this._progressOperation.Tick();
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
