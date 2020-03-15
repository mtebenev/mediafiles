using System.Threading.Tasks;

namespace Mt.MediaMan.AppEngine.Cataloging
{
  /// <summary>
  /// Thin interface for catalog tasks.
  /// </summary>
  public interface ICatalogTask
  {
    internal Task ExecuteAsync(Catalog catalog);
  }
}
