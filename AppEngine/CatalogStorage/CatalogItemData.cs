using Newtonsoft.Json.Linq;

namespace Mt.MediaMan.AppEngine.CatalogStorage
{
  /// <summary>
  /// Contains info parts associated with catalog item. Stored as YesSql document.
  /// Example of info parts are: video, book, scan root etc
  /// </summary>
  public class CatalogItemData
  {
    public CatalogItemData(int catalogItemId)
    {
      CatalogItemId = catalogItemId;
      Data = new JObject();
    }

    public int CatalogItemId { get; }
    public JObject Data { get; set; }
  }
}
