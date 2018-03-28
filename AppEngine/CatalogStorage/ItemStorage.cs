using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Mt.MediaMan.AppEngine.Cataloging;
using YesSql;
using YesSql.Provider.SqlServer;

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
    }

    public Task InitializeAsync()
    {
      return _store.InitializeAsync();
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
    public Task SaveInfoPartAsync<TPart>(int itemId, TPart infoPart)
    {
      using(var session = _store.CreateSession())
      {
        session.Save(infoPart);
      }

      return Task.CompletedTask;
    }

    /// <summary>
    /// IDisposable
    /// </summary>
    public void Dispose()
    {
      _dbConnection?.Dispose();
      _store?.Dispose();
    }
  }
}
