using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YesSql;

namespace Mt.MediaFiles.AppEngine.CatalogStorage
{
  /// <summary>
  /// Store module-specific data using the module storage
  /// </summary>
  public class ModuleStorage
  {
    private readonly IStore _store;

    public ModuleStorage(IStore store)
    {
      _store = store;
    }

    public Task SaveDocumentAsync<TDocument>(TDocument document) where TDocument : class
    {
      using(var session = _store.CreateSession())
      {
        session.Save(document);
      }

      return Task.CompletedTask;
    }

    public async Task<IList<TDocument>> QueryDocumentsAsync<TDocument>() where TDocument : class
    {
      IList<TDocument> result;

      using(var session = _store.CreateSession())
      {
        var documents = await session.Query<TDocument>()
          .ListAsync();

        result = documents.ToList();
      }

      return result;
    }
  }
}
