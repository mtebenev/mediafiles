using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Scanning;
using YesSql;
using YesSql.Provider.SqlServer;
using YesSql.Sql;

namespace Mt.MediaMan.AppEngine.CatalogStorage
{
  internal class ItemStorage : IItemStorage
  {
    private readonly IDbConnection _dbConnection;
    private readonly Store _store;

    public ItemStorage(string connectionString)
    {
      _dbConnection = new SqlConnection(connectionString);

      // Document store
      var storeConfiguration = new Configuration();
      storeConfiguration.UseSqlServer(connectionString, IsolationLevel.ReadUncommitted);

      _store = new Store(storeConfiguration);
      _store.RegisterIndexes<CatalogItemIndexProvider>();
    }

    public async Task InitializeAsync()
    {
      // Check if catalog item table exists
      var checkTableQuery = @"SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'CatalogItem'";
      var tables = await _dbConnection.QueryAsync(checkTableQuery);
      if(!tables.Any())
      {
        // Catalog item table
        var query = @"CREATE TABLE [dbo].[CatalogItem] (
    [CatalogItemId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]          NVARCHAR (256) NOT NULL,
    [Size]          INT            NULL,
    [ParentItemId]  INT            NOT NULL,
    [ItemType]      VARCHAR (4)    NOT NULL
);";
        await _dbConnection.ExecuteAsync(query);

        // Document storage
        await _store.InitializeAsync();

        using(var session = _store.CreateSession())
        {
          var schemaBuilder = new SchemaBuilder(session);

          schemaBuilder.CreateMapIndexTable(nameof(MapIndexCatalogItem), table => table
            .Column<int>(nameof(MapIndexCatalogItem.CatalogItemId)));
        }

        // The root item
        await SaveRootItemAsync();
      }
    }

    /// <summary>
    /// IItemStorage
    /// </summary>
    public async Task<int> CreateItemAsync(CatalogItemRecord itemRecord)
    {
      int itemId = await _dbConnection.InsertAsync(itemRecord);
      return itemId;
    }

    /// <summary>
    /// IItemStorage
    /// </summary>
    public async Task<CatalogItemRecord> LoadRootItemAsync()
    {
      var query = @"select * from CatalogItem where [ItemType]=@ItemType";
      var rootItemRecord = await _dbConnection.QueryFirstOrDefaultAsync<CatalogItemRecord>(query, new {ItemType = CatalogItemType.CatalogRoot});

      if(rootItemRecord == null)
        throw new InvalidOperationException("Cannot load root item in the catalog");

      return rootItemRecord;
    }

    /// <summary>
    /// IItemStorage
    /// </summary>
    public async Task<CatalogItemRecord> LoadItemByIdAsync(int catalogItemId)
    {
      var query = @"select * from CatalogItem where [CatalogItemId]=@CatalogItemId";
      var itemRecord = await _dbConnection.QueryFirstAsync<CatalogItemRecord>(query, new {CatalogItemId = catalogItemId});

      return itemRecord;
    }

    /// <summary>
    /// IItemStorage
    /// </summary>
    public async Task<IList<CatalogItemRecord>> LoadChildrenAsync(int parentItemId)
    {
      var query = @"select * from CatalogItem where [ParentItemId]=@ParentItemId";
      var itemRecords = await _dbConnection.QueryAsync<CatalogItemRecord>(query, new {ParentItemId = parentItemId});

      return itemRecords.ToList();
    }

    /// <summary>
    /// IItemStorage
    /// </summary>
    public Task SaveItemDataAsync(int catalogItemId, CatalogItemData itemData)
    {
      using(var session = _store.CreateSession())
      {
        session.Save(itemData);
      }

      return Task.CompletedTask;
    }

    /// <summary>
    /// IItemStorage
    /// </summary>
    public async Task<CatalogItemData> LoadItemDataAsync(int catalogItemId)
    {
      CatalogItemData result = null;

      using(var session = _store.CreateSession())
      {
        var itemDatas = await session.Query<CatalogItemData, MapIndexCatalogItem>()
          .Where(x => x.CatalogItemId == catalogItemId)
          .ListAsync();

        if(itemDatas == null)
          throw new InvalidOperationException();

        var itemDataList = itemDatas.ToList();

        if(itemDataList.Count > 1)
          throw new InvalidOperationException();


        result = itemDataList.FirstOrDefault();
      }

      return result;
    }

    /// <summary>
    /// IItemStorage
    /// </summary>
    public async Task<IList<int>> SearchItemsAsync(string whereFilter)
    {
      var query = @"select [CatalogItemId] from CatalogItem where [Name] like @NameFilter";
      var escapedFilter = whereFilter
        .Replace("_", "[_]")
        .Replace('?', '_')
        .Replace('*', '%');

      var result = await _dbConnection.QueryAsync<int>(query, new {NameFilter = escapedFilter});

      return result.ToList();
    }

    /// <summary>
    /// IDisposable
    /// </summary>
    public void Dispose()
    {
      _dbConnection?.Dispose();
      _store?.Dispose();
    }

    private Task SaveRootItemAsync()
    {
      var catalogItem = new CatalogItemRecord
      {
        ItemType = CatalogItemType.CatalogRoot,
        Name = "[ROOT]"
      };

      return CreateItemAsync(catalogItem);
    }
  }
}
