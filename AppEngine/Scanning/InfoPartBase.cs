using Mt.MediaMan.AppEngine.CatalogStorage;
using Newtonsoft.Json;

namespace Mt.MediaMan.AppEngine.Scanning
{
  /// <summary>
  /// Common interface for all info parts
  /// </summary>
  public abstract class InfoPartBase
  {
    [JsonIgnore]
    internal CatalogItemData CatalogItemData { get; set; }
  }
}
