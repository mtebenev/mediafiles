using System.Data;

namespace Mt.MediaFiles.AppEngine.Common
{
  /// <summary>
  /// An abstract db connection access.
  /// </summary>
  public interface IDbConnectionProvider
  {
    /// <summary>
    /// Provides db connection.
    /// </summary>
    IDbConnection GetConnection();

    /// <summary>
    /// Provides YesSql configuration.
    /// </summary>
    YesSql.IConfiguration GetYesSqlConfiguration();
  }
}
