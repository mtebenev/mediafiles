using Microsoft.Extensions.Logging;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using Mt.MediaFiles.AppEngine.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Mt.MediaFiles.AppEngine.Scanning
{
  /// <summary>
  /// Scanning pipeline.
  /// </summary>
  internal static class Pipeline
  {
    /// <summary>
    /// The factory method.
    /// </summary>
    public static (ITargetBlock<CatalogItemRecord>, ITargetBlock<CatalogItemRecord>) Create(IScanContext scanContext, IProgressOperation progressOperation, ILoggerFactory loggerFactory)
    {
      var scanSvcBlock = BlockScanSvc.Create(scanContext, progressOperation, loggerFactory);
      return (scanSvcBlock, scanSvcBlock);
    }
  }
}
