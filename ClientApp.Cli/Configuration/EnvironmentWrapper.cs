using System;

namespace Mt.MediaFiles.ClientApp.Cli.Configuration
{
  /// <summary>
  /// Wraps .net core Environment for testability.
  /// </summary>
  internal class EnvironmentWrapper : IEnvironment
  {
    /// <summary>
    /// IEnvironment.
    /// </summary>
    public string GetDataPath()
    {
      return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    }
  }
}
