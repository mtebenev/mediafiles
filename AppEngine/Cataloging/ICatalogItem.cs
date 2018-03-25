using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mt.MediaMan.AppEngine.Cataloging
{
  public interface ICatalogItem
  {
    int CatalogItemId { get; }
    
    /// <summary>
    /// Usually corresponds to a file name
    /// </summary>
    string Name { get; }

    // Navigation
    Task<ICatalogItem> GetParentItemAsync();
    Task<IList<ICatalogItem>> GetChildrenAsync();
  }
}
