using FluentAssertions;
using Microsoft.Data.Sqlite;
using Mt.MediaFiles.AppEngine.Video.Thumbnail;
using System.Threading.Tasks;
using Xunit;

namespace Mt.MediaFiles.AppEngine.Video.Test.Thumbnail
{
  public class ThumbnailStorageTest
  {
    [Fact]
    public async Task Load_Item_Thumbnails()
    {
      var (connection, storage) = await this.CreateTestStorageAsync();
      using (connection)
      {
        var records = new[]
        {
          new ThumbnailRecord {CatalogItemId = 10, Offset = 10, ThumbnailData = new byte[] { 1, 1, 1 } },
          new ThumbnailRecord {CatalogItemId = 11, Offset = 20, ThumbnailData = new byte[] { 2, 2, 2 } },
          new ThumbnailRecord {CatalogItemId = 12, Offset = 30, ThumbnailData = new byte[] { 3, 3, 3 } },
        };
        await storage.SaveRecordsAsync(records);

        var result = await storage.GetCatalogItemRecordsAsync(records[1].CatalogItemId);
        Assert.Equal(1, result.Count);
        result[0].Should()
          .BeEquivalentTo(
          records[1],
          options => options.Excluding(x => x.ThumbnailId));
      }
    }

    private async Task<(SqliteConnection, ThumbnailStorage)> CreateTestStorageAsync()
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

      var storage = new ThumbnailStorage(connection);

      return (connection, storage);
    }
  }
}
