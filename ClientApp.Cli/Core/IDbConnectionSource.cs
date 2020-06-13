namespace Mt.MediaFiles.ClientApp.Cli.Core
{
  /// <summary>
  /// Setter interface for db connection provider.
  /// </summary>
  internal interface IDbConnectionSource
  {
    /// <summary>
    /// Sets the new connection string to the connection provider.
    /// </summary>
    void SetConnectionString(string connectionString);
  }
}
