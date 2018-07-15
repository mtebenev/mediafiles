using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.CatalogStorage;
using YesSql;
using YesSql.Sql;

namespace Mt.MediaMan.AppEngine.Ebooks.Storage
{
  internal class ModuleStorageProvider : IModuleStorageProvider
  {
    public Task InitializeStoreAsync(ISession session, SchemaBuilder schemaBuilder)
    {
      schemaBuilder.CreateMapIndexTable(nameof(MapIndexEbook), table => table
        .Column<string>(nameof(MapIndexEbook.EbookId)));

      session.Store.RegisterIndexes<EbookIndexProvider>();
      return Task.CompletedTask;
    }
  }
}
