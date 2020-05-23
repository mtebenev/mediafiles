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

    /// <summary>
    /// IEnvironment.
    /// </summary>
    public string GetEnvironmentVariable(string name)
    {
      return Environment.GetEnvironmentVariable(name);
    }

    /// <summary>
    /// IEnvironment.
    /// </summary>
    public string GetBaseDirectory()
    {
      return AppContext.BaseDirectory;
    }
  }
}
