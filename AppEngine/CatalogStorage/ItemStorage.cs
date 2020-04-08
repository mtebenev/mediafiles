using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Mt.MediaFiles.AppEngine.Cataloging;
using YesSql;
using YesSql.Sql;

namespace Mt.MediaFiles.AppEngine.CatalogStorage
{
  internal class ItemStorage : IItemStorage
  {
    private readonly IStorageManager _storageManager;

    private IDbConnection DbConnection => _storageManager.DbConnection;
    private IStore Store => _storageManager.Store;

    public ItemStorage(IStorageManager storageManager)
    {
      _storageManager = storageManager;
    }

    public async Task InitializeAsync(IReadOnlyList<IModuleStorageProvider> moduleStorageProviders)
    {
      // Check if catalog item table exists
      var isTableExists = await DbUtils.IsTableExistsAsync(this.DbConnection, "CatalogItem");
      if(!isTableExists)
      {
        // Catalog item table
        var query = @"CREATE TABLE CatalogItem (
                        CatalogItemId   INTEGER        NOT NULL PRIMARY KEY AUTOINCREMENT,
                        Path            NVARCHAR (256) NOT NULL,
                        ItemType        VARCHAR (4)    NOT NULL,
                        Size            BIGINT         NULL,
                        ParentItemId    INTEGER        NOT NULL
);";
        await this.DbConnection.ExecuteAsync(query);

        // Document storage
        await InitializeStoreAsync(moduleStorageProviders);

        // The root item
        await this.SaveRootItemAsync();
      }
    }

    /// <summary>
    /// IItemStorage.
    /// </summary>
    public async Task<int> CreateItemAsync(CatalogItemRecord itemRecord)
    {
      var itemId = await this.DbConnection.InsertAsync(itemRecord);
      return itemId;
    }

    /// <summary>
    /// IItemStorage.
    /// </summary>
    public Task CreateManyItemsAsync(IEnumerable<CatalogItemRecord> records)
    {
      try
      {
        this.DbConnection.Open();
        using(var transaction = this.DbConnection.BeginTransaction())
        {
          foreach(var r in records)
          {
            this.DbConnection.Insert(r, transaction);
          }
          transaction.Commit();
        }
      }
      finally
      {
        this.DbConnection.Close();
      }

      return Task.CompletedTask;
    }

    /// <summary>
    /// IItemStorage.
    /// </summary>
    public async Task<CatalogItemRecord> LoadRootItemAsync()
    {
      var query = @"SELECT * FROM CatalogItem WHERE ItemType=@ItemType";
      var rootItemRecord = await this.DbConnection.QueryFirstOrDefaultAsync<CatalogItemRecord>(query, new { ItemType = CatalogItemType.CatalogRoot });

      if(rootItemRecord == null)
        throw new InvalidOperationException("Cannot load root item in the catalog");

      return rootItemRecord;
    }

    /// <summary>
    /// IItemStorage.
    /// </summary>
    public async Task<CatalogItemRecord> LoadItemByIdAsync(int catalogItemId)
    {
      var query = @"SELECT * FROM CatalogItem WHERE CatalogItemId=@CatalogItemId";
      var itemRecord = await this.DbConnection.QueryFirstAsync<CatalogItemRecord>(query, new { CatalogItemId = catalogItemId });

      return itemRecord;
    }

    /// <summary>
    /// IItemStorage.
    /// </summary>
    public async Task<IList<CatalogItemRecord>> LoadChildrenAsync(int parentItemId)
    {
      var query = @"SELECT * FROM CatalogItem WHERE ParentItemId=@ParentItemId";
      var itemRecords = await this.DbConnection.QueryAsync<CatalogItemRecord>(query, new { ParentItemId = parentItemId });

      return itemRecords.ToList();
    }

    /// <summary>
    /// IItemStorage.
    /// </summary>
    public Task SaveItemDataAsync(int catalogItemId, CatalogItemData itemData)
    {
      using(var session = Store.CreateSession())
      {
        session.Save(itemData);
      }

      return Task.CompletedTask;
    }

    /// <summary>
    /// IItemStorage.
    /// </summary>
    public async Task<CatalogItemData> LoadItemDataAsync(int catalogItemId)
    {
      CatalogItemData result = null;

      using(var session = Store.CreateSession())
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
    /// IItemStorage.
    /// </summary>
    public async Task<IList<int>> SearchItemsAsync(string whereFilter)
    {
      var query = @"SELECT CatalogItemId FROM CatalogItem WHERE Path LIKE @NameFilter";
      var escapedFilter = whereFilter
        .Replace("_", "[_]")
        .Replace('?', '_')
        .Replace('*', '%');

      var result = await DbConnection.QueryAsync<int>(query, new { NameFilter = escapedFilter });

      return result.ToList();
    }

    /// <summary>
    /// IItemStorage.
    /// </summary>
    public async Task<IList<CatalogItemRecord>> QuerySubtree(CatalogItemLocation catalogItemLocation)
    {
      var query = @"SELECT * FROM CatalogItem WHERE ParentItemId=@ScanRootId AND Path LIKE @PathPrefix";
      var itemRecords = await this.DbConnection
        .QueryAsync<CatalogItemRecord>(query,
        new
        {
          catalogItemLocation.ScanRootId,
          PathPrefix = $"{catalogItemLocation.PathPrefix}%"
        });

      var result = itemRecords.ToList();
      return result;
    }

    private Task SaveRootItemAsync()
    {
      var catalogItem = new CatalogItemRecord
      {
        ItemType = CatalogItemType.CatalogRoot,
        Path = "[ROOT]"
      };

      return CreateItemAsync(catalogItem);
    }

    private async Task InitializeStoreAsync(IReadOnlyList<IModuleStorageProvider> moduleStorageProviders)
    {
      await Store.InitializeAsync();

      using(var session = Store.CreateSession())
      {
        var schemaBuilder = new SchemaBuilder(session);

        schemaBuilder.CreateMapIndexTable(nameof(MapIndexCatalogItem), table => table
          .Column<int>(nameof(MapIndexCatalogItem.CatalogItemId)));

        foreach(var provider in moduleStorageProviders)
        {
          await provider.InitializeStoreAsync(session, schemaBuilder);
        }
      }
    }
  }
}
