using Microsoft.Data.Sqlite;
using Mt.MediaFiles.AppEngine.Video.Thumbnail;
using System.Threading.Tasks;

namespace Mt.MediaFiles.AppEngine.Video.Test.Thumbnail
{
  public class ThumbnailStorageTest
  {
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
