using System.Collections.Generic;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Scanning;

namespace Mt.MediaMan.AppEngine.Cataloging
{
  public interface ICatalogItem
  {
    int CatalogItemId { get; }

    /// <summary>
    /// Item type-specific name (file name for FS items).
    /// </summary>
    string Name { get; }

    /// <summary>
    /// The item path (for files) or item type-specific string.
    /// </summary>
    string Path { get; }

    /// <summary>
    /// Size of the item (-1 for directories)
    /// </summary>
    long Size { get; }

    /// <summary>
    /// True if the item is directory.
    /// </summary>
    bool IsDirectory { get; }

    /// <summary>
    /// <see cref="CatalogItemType"/>
    /// </summary>
    string ItemType { get; }

    // Navigation
    Task<IList<ICatalogItem>> GetChildrenAsync();

    // Info parts
    Task<TInfoPart> GetInfoPartAsync<TInfoPart>() where TInfoPart : InfoPartBase;

    /// <summary>
    /// Get names of all info parts for the item
    /// </summary>
    Task<IList<string>> GetInfoPartNamesAsync();
  }
}
