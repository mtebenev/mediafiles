using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mt.MediaFiles.AppEngine.CatalogStorage
{
  internal static class CatalogItemDataSettings
  {
    /// <summary>
    /// Replace current value, even for null values, union arrays.
    /// </summary>
    public static readonly JsonMergeSettings JsonMergeSettings = new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Union, MergeNullValueHandling = MergeNullValueHandling.Merge };

    /// <summary>
    /// A Json serializer that ignores properties which have their default values.
    /// </summary>
    public static readonly JsonSerializer IgnoreDefaultValuesSerializer = new JsonSerializer { DefaultValueHandling = DefaultValueHandling.Ignore };
  }
}
