using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.Scanning;
using System.Threading.Tasks;

namespace Mt.MediaFiles.AppEngine.Matching
{
  /// <summary>
  /// Property access for catalog items for matching.
  /// </summary>
  public class InfoPartAccessCatalogItem : IInfoPartAccess
  {
    private readonly ICatalog _catalog;

    /// <summary>
    /// Ctor.
    /// </summary>
    public InfoPartAccessCatalogItem(ICatalog catalog)
    {
      this._catalog = catalog;
    }

    /// <summary>
    /// IInfoPartAccess.
    /// </summary>
    public async Task<FileProperties> GetFilePropertiesAsync(int itemId)
    {
      var catalogItem = await this._catalog.GetItemByIdAsync(itemId);
      var result = new FileProperties
      {
        Path = catalogItem.Path,
        Size = catalogItem.Size
      };

      return result;
    }

    /// <summary>
    /// IInfoPartAccess.
    /// </summary>
    public async Task<TInfoPart> GetInfoPartAsync<TInfoPart>(int itemId) where TInfoPart : InfoPartBase
    {
      var catalogItem = await this._catalog.GetItemByIdAsync(itemId);
      var result = await catalogItem.GetInfoPartAsync<TInfoPart>();

      return result;
    }
  }
}
