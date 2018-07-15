using System.Collections.Generic;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Ebooks.Storage;

namespace Mt.MediaMan.AppEngine.Ebooks.Commands
{
  /// <summary>
  /// Creates a new ebook record using book part of a given catalog item
  /// </summary>
  public class CommandSearchBook
  {
    public async Task<IList<EbookDocument>> ExecuteAsync(Catalog catalog)
    {
      var moduleStorage = catalog.CreateModuleStorage();
      var ebookStorage = new EbookStorage(moduleStorage);

      // Query documents
      var result = await ebookStorage.QueryDocumentsAsync();
      return result;
    }
  }
}
