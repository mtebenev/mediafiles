using System.Collections.Generic;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Cataloging;

namespace Mt.MediaMan.AppEngine.Commands
{
  /// <summary>
  /// Performs search in catalog
  /// </summary>
  public class CommandSearch
  {
    public Task<IList<int>> Execute(Catalog catalog, string query)
    {
      return catalog.SearchAsync(query);
    }
  }
}
