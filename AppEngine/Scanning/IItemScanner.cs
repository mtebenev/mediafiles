using System.Threading.Tasks;

namespace Mt.MediaMan.AppEngine.Scanning
{
  /// <summary>
  /// Scans something and adds to catalog
  /// </summary>
  internal interface IItemScanner
  {
    Task Scan(IScanContext scanContext);
  }
}
