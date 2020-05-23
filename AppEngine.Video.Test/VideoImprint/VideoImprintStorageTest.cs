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
      var (connection, storage) = await this.CreateTestStorageAsync();
      using(connection)
      {
        var records = new[]
        {
          new VideoImprintRecord {CatalogItemId = 10, ImprintData = new byte[0] },
          new VideoImprintRecord {CatalogItemId = 11, ImprintData = new byte[0] },
          new VideoImprintRecord {CatalogItemId = 12, ImprintData = new byte[0] },
        };
        await storage.SaveRecordsAsync(records);

        var result = await storage.GetAllRecordsAsync();
        Assert.Equal(new[] { 10, 11, 12 }, result.Select(r => r.CatalogItemId).ToList());
      }
    }

    [Fact]
    public async Task Delete_Item_Records()
    {
      var (connection, storage) = await this.CreateTestStorageAsync();
      using(connection)
      {
        var records = new[]
        {
          new VideoImprintRecord {CatalogItemId = 10, ImprintData = new byte[] { 1, 2, 3 } },
          new VideoImprintRecord {CatalogItemId = 10, ImprintData = new byte[] { 4, 5, 6 } },
          new VideoImprintRecord {CatalogItemId = 10, ImprintData = new byte[] { 7, 8, 9 } },
        };
        await storage.SaveRecordsAsync(records);

        await storage.DeleteRecordsAsync(10);

        var resultRecords = await storage.GetAllRecordsAsync();
        resultRecords.Should().BeEmpty();
      }
    }

    private async Task<(SqliteConnection, VideoImprintStorage)> CreateTestStorageAsync()
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

      var storage = new VideoImprintStorage(connection);

      return (connection, storage);
    }
  }
}
