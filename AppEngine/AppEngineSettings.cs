namespace Mt.MediaFiles.AppEngine
{
  /// <summary>
  /// Provides settings for the engine. Should be available in DI container.
  /// </summary>
  public class AppEngineSettings
  {
    /// <summary>
    /// Ctor.
    /// </summary>
    public AppEngineSettings(string dataDirectory)
    {
      this.DataDirectory = dataDirectory;
    }

    /// <summary>
    /// The absolute path with the data directory (lucene indexes etc).
    /// </summary>
    public string DataDirectory { get; private set; }
  }
}
