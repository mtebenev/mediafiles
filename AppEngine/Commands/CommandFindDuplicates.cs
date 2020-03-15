using System.Collections.Generic;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Tools;

namespace Mt.MediaMan.AppEngine.Commands
{
  /// <summary>
  /// Finds duplicates in the catalog
  /// </summary>
  public class CommandFindDuplicates
  {
    public async Task<IList<DuplicateFindResult>> Execute(ICatalog catalog)
    {
      var finder = new DuplicateFinder(catalog);
      var result = await finder.FindAsync();

      return result;
    }
  }
}
