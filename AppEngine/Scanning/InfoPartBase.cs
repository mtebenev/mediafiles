using Mt.MediaFiles.AppEngine.CatalogStorage;
using Newtonsoft.Json;

namespace Mt.MediaFiles.AppEngine.Scanning
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
