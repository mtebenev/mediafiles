using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AppEngine.Video.VideoImprint;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Mt.MediaFiles.AppEngine.Video.VideoImprint;
using Xunit;

namespace Mt.MediaFiles.AppEngine.Video.Test.VideoImprint
{
  public class VideoImprintStorageTest
  {
    [Fact]
    public async Task Get_Catalog_Item_Ids()
    {
      var (connection, storage) = await this.CreateTestStorageAsync(10);
      using(connection)
      {
        var records = new[]
        {
          new VideoImprintRecord {CatalogItemId = 10, ImprintData = new byte[0] },
          new VideoImprintRecord {CatalogItemId = 11, ImprintData = new byte[0] },
          new VideoImprintRecord {CatalogItemId = 12, ImprintData = new byte[0] },
        };

        for(int i = 0; i < records.Length; i++)
        {
          await storage.SaveRecordAsync(records[i]);
        }
        await storage.FlushAsync();

        var result = await storage.GetAllRecordsAsync();
        Assert.Equal(new[] { 10, 11, 12 }, result.Select(r => r.CatalogItemId).ToList());
      }
    }

    [Fact]
    public async Task Delete_Item_Records()
    {
      var (connection, storage) = await this.CreateTestStorageAsync(10);
      using(connection)
      {
        var records = new[]
        {
          new VideoImprintRecord {CatalogItemId = 10, ImprintData = new byte[] { 1, 2, 3 } },
          new VideoImprintRecord {CatalogItemId = 10, ImprintData = new byte[] { 4, 5, 6 } },
          new VideoImprintRecord {CatalogItemId = 10, ImprintData = new byte[] { 7, 8, 9 } },
        };
        for(int i = 0; i < records.Length; i++)
        {
          await storage.SaveRecordAsync(records[i]);
        }

        await storage.DeleteRecordsAsync(10);

        var resultRecords = await storage.GetAllRecordsAsync();
        resultRecords.Should().BeEmpty();
      }
    }

    [Fact]
    public async Task Should_Save_Records_On_Flush()
    {
      var sourceRecords = new[]
      {
        new VideoImprintRecord {CatalogItemId = 10, ImprintData = new byte[0] },
        new VideoImprintRecord {CatalogItemId = 11, ImprintData = new byte[0] },
      };

      var (connection, storage) = await this.CreateTestStorageAsync(10);
      for(int i = 0; i < sourceRecords.Length; i++)
      {
        await storage.SaveRecordAsync(sourceRecords[i]);
      }

      var records = await storage.GetAllRecordsAsync();
      Assert.Empty(records);


      await storage.FlushAsync();
      records = await storage.GetAllRecordsAsync();
      records.Select(r => r.CatalogItemId).Should().BeEquivalentTo(new[] { 10, 11 });
    }

    [Fact]
    public async Task Should_Flush_Intermediate_Blocks()
    {
      var (connection, storage) = await this.CreateTestStorageAsync(2);
      await storage.SaveRecordAsync(new VideoImprintRecord { CatalogItemId = 10, ImprintData = new byte[0] });
      await storage.SaveRecordAsync(new VideoImprintRecord { CatalogItemId = 11, ImprintData = new byte[0] });
      await storage.SaveRecordAsync(new VideoImprintRecord { CatalogItemId = 12, ImprintData = new byte[0] });

      var records = await storage.GetAllRecordsAsync();
      records.Select(r => r.CatalogItemId).Should().BeEquivalentTo(new[] { 10, 11 });
    }


    private async Task<(SqliteConnection, VideoImprintStorage)> CreateTestStorageAsync(int bufferSize)
    {
      var connectionString = new SqliteConnectionStringBuilder
      {
        Mode = SqliteOpenMode.Memory,
        DataSource = "mem"
      }.ToString();

      var connection = new SqliteConnection(connectionString);
      connection.Open();

      var moduleDbProvider = new ModuleDbProvider();
      await moduleDbProvider.InitializeDbAsync(connection);

      var storage = new VideoImprintStorage(connection, bufferSize);

      return (connection, storage);
    }
  }
}
