using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using Mt.MediaFiles.AppEngine.Common;
using NSubstitute;
using Xunit;
using YesSql;
using YesSql.Provider.Sqlite;

namespace Mt.MediaFiles.AppEngine.Test.CatalogStorage
{
  /// <summary>
  /// Note: these tests are heavey: we use real store and in-memory db for testing.
  /// </summary>
  public class ItemStorageTest
  {
    [Fact]
    public async Task Should_Load_Root_Item()
    {
      var storage = await this.CreateTestStorageAsync();
      var record = await storage.LoadRootItemAsync();
      record.Should().BeEquivalentTo(new CatalogItemRecord
      {
        CatalogItemId = 1,
        Path = "[ROOT]",
        ItemType = CatalogItemType.CatalogRoot,
      });
    }

    [Fact]
    public async Task Should_Load_Item_By_Id()
    {
      var storage = await this.CreateTestStorageAsync();
      var record = new CatalogItemRecord
      {
        ItemType = CatalogItemType.File,
        Path = "some_path",
        ParentItemId = 1,
        Size = 100
      };

      var recordId = await storage.CreateItemAsync(record);
      var loadedRecord = await storage.LoadItemByIdAsync(recordId);

      loadedRecord.Should().BeEquivalentTo(record);
    }

    [Fact]
    public async Task Should_Query_Subtree()
    {
      var storage = await this.CreateTestStorageAsync();
      var scanRootId = await storage.CreateItemAsync(new CatalogItemRecord { ItemType = CatalogItemType.ScanRoot, ParentItemId = 1, Path = "scan_root" });

      var childRecords = new[]
      {
        new CatalogItemRecord { ItemType = CatalogItemType.File, ParentItemId = scanRootId, Path = @"x:\folder1\file1.txt" },
        new CatalogItemRecord { ItemType = CatalogItemType.File, ParentItemId = scanRootId, Path = @"x:\folder1\file2.txt" },
        new CatalogItemRecord { ItemType = CatalogItemType.File, ParentItemId = scanRootId, Path = @"x:\folder2\file1.txt" },
      };

      foreach(var r in childRecords)
      {
        await storage.CreateItemAsync(r);
      }

      // Verify
      var loadedRecords = await storage.QuerySubtree(new CatalogItemLocation(scanRootId, @"x:\folder1"));
      loadedRecords.Should().BeEquivalentTo(childRecords.Take(2));
    }


    private async Task<ItemStorage> CreateTestStorageAsync()
    {
      var connectionString = new SqliteConnectionStringBuilder
      {
        Mode = SqliteOpenMode.Memory,
        DataSource = "mem"
      }.ToString();

      var storeConfiguration = new Configuration();
      storeConfiguration.UseSqLite(connectionString, IsolationLevel.ReadUncommitted, true);
      var connection = storeConfiguration.ConnectionFactory.CreateConnection();
      connection.Open();
      var dbConnectionProvider = Substitute.For<IDbConnectionProvider>();
      dbConnectionProvider.GetConnection().Returns(connection);

      var storageManager = new StorageManager(dbConnectionProvider, storeConfiguration);

      var storageProviders = new List<IModuleStorageProvider>();
      var storage = new ItemStorage(storageManager);
      await storage.InitializeAsync(storageProviders);

      return storage;
    }
  }
}
