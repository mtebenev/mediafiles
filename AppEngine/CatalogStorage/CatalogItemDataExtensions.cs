using System;
using Mt.MediaMan.AppEngine.Scanning;
using Newtonsoft.Json.Linq;

namespace Mt.MediaMan.AppEngine.CatalogStorage
{
  internal static class CatalogItemDataExtensions
  {
    /// <summary>
    /// Gets a info part by its name.
    /// </summary>
    public static TInfoPart Get<TInfoPart>(this CatalogItemData catalogItemData) where TInfoPart : InfoPartBase
    {
      var result = (TInfoPart)catalogItemData.Get(typeof(TInfoPart), typeof(TInfoPart).Name);
      return result;
    }

    /// <summary>
    /// Gets a info part by its name.
    /// </summary>
    public static InfoPartBase Get(this CatalogItemData catalogItemData, Type infoPartType, string name)
    {
      var elementData = catalogItemData.Data[name] as JObject;
      InfoPartBase result = (InfoPartBase) elementData?.ToObject(infoPartType);

      if(result != null)
        result.CatalogItemData = catalogItemData;

      return result;
    }

    /// <summary>
    /// Get a new one or existing info part
    /// </summary>
    public static TInfoPart GetOrCreate<TInfoPart>(this CatalogItemData catalogItemData) where TInfoPart : InfoPartBase, new()
    {
      var result = catalogItemData.GetOrCreate<TInfoPart>(typeof(TInfoPart).Name);
      return result;
    }

    /// <summary>
    /// Get a new one or existing info part
    /// </summary>
    public static TInfoPart GetOrCreate<TInfoPart>(this CatalogItemData catalogItemData, string name) where TInfoPart : InfoPartBase, new()
    {
      var existing = catalogItemData.Get<TInfoPart>() ?? new TInfoPart {CatalogItemData = catalogItemData};
      return existing;
    }

    /// <summary>
    /// Updates the catalog item data with info part content. Invoke after chaning properties of the info part.
    /// </summary>
    public static CatalogItemData Apply<TInfoPart>(this CatalogItemData catalogItemData, TInfoPart infoPart) where TInfoPart : InfoPartBase
    {
      catalogItemData.Apply(typeof(TInfoPart).Name, infoPart);
      return catalogItemData;
    }

    public static CatalogItemData Apply(this CatalogItemData catalogItemData, string name, InfoPartBase infoPart)
    {
      var existingData = catalogItemData.Data[name] as JObject;

      if(existingData != null)
        existingData.Merge(JObject.FromObject(infoPart), CatalogItemDataSettings.JsonMergeSettings);
      else
        catalogItemData.Data[name] = JObject.FromObject(infoPart, CatalogItemDataSettings.IgnoreDefaultValuesSerializer);

      return catalogItemData;
    }

  }
}
