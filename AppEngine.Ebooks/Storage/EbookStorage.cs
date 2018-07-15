using System.Collections.Generic;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.CatalogStorage;

namespace Mt.MediaMan.AppEngine.Ebooks.Storage
{
  /// <summary>
  /// Ebook-related storage operations over base storage
  /// </summary>
  internal class EbookStorage
  {
    private readonly ModuleStorage _moduleStorage;

    public EbookStorage(ModuleStorage moduleStorage)
    {
      _moduleStorage = moduleStorage;
    }

    public Task SaveDocumentAsync(EbookDocument document)
    {
      _moduleStorage.SaveDocumentAsync(document);
      return Task.CompletedTask;
    }

    public Task<IList<EbookDocument>> QueryDocumentsAsync()
    {
      return _moduleStorage.QueryDocumentsAsync<EbookDocument>();
    }
  }
}
