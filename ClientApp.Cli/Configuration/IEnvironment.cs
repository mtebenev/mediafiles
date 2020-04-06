namespace Mt.MediaFiles.ClientApp.Cli.Configuration
{
  /// <summary>
  /// Testable interface for .net core environment
  /// </summary>
  public interface IEnvironment
  {
    /// <summary>
    /// Returns path the user data directory.
    /// </summary>
    string GetDataPath();
  }
}
