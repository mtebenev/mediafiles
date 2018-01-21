using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.CatalogStorage;

namespace Mt.MediaMan.AppEngine.Scanning
{
  /// <summary>
  /// Scans something and adds to catalog
  /// </summary>
  internal interface IItemScanner
  {
    Task Scan(IItemStorage itemStorage);
  }
}
