using System.Collections.Generic;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Cataloging;

namespace Mt.MediaMan.AppEngine.Commands
{
  /// <summary>
  /// Performs search for files in catalog
  /// </summary>
  public class CommandSearchFiles
  {
    public Task<IList<int>> ExecuteAsync(Catalog catalog, string query)
    {
      return catalog.SearchFilesAsync(query);
    }
  }
}
