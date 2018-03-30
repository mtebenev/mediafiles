using System.Collections.Generic;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Scanning;

namespace Mt.MediaMan.AppEngine.Cataloging
{
  public interface ICatalogItem
  {
    int CatalogItemId { get; }
    
    /// <summary>
    /// Usually corresponds to a file name
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Size of the item (0 for directories)
    /// </summary>
    int Size { get; }

    // Navigation
    Task<ICatalogItem> GetParentItemAsync();
    Task<IList<ICatalogItem>> GetChildrenAsync();

    // Info parts
    Task<InfoPartVideo> GetInfoPartAsync();
  }
}
