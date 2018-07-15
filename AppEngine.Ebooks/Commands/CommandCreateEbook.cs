using System.Linq;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Ebooks.Storage;
using Mt.MediaMan.AppEngine.Scanning;

namespace Mt.MediaMan.AppEngine.Ebooks.Commands
{
  /// <summary>
  /// Creates a new ebook record using book part of a given catalog item
  /// </summary>
  public class CommandCreateEbook
  {
    public async Task Execute(Catalog catalog, int catalogItemId)
    {
      var moduleStorage = catalog.CreateModuleStorage();
      var ebookStorage = new EbookStorage(moduleStorage);

      var walker = CatalogTreeWalker.CreateDefaultWalker(catalog, catalogItemId, item => ProcessItem(item, ebookStorage));
      await walker.ForEachAsync(item => {});
    }

    private async Task ProcessItem(ICatalogItem item, EbookStorage ebookStorage)
    {
      var infoPartBook = await item.GetInfoPartAsync<InfoPartBook>();
      if(infoPartBook != null)
      {
        var document = new EbookDocument
        {
          Title = infoPartBook.Title,
          Authors = infoPartBook.Authors.ToArray(),
          Isbn = infoPartBook.Isbn
        };

        // Save document
        await ebookStorage.SaveDocumentAsync(document);
      }
    }
  }
}
