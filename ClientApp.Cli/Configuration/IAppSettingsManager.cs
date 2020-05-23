using Mt.MediaFiles.AppEngine;

namespace Mt.MediaFiles.ClientApp.Cli.Configuration
{

  /// <summary>
  /// Utility class keeping track of the settings source and able to update the settings.
  /// </summary>
  internal interface IAppSettingsManager
  {
    /// <summary>
    /// The app settings.
    /// </summary>
    AppSettings AppSettings { get; }

    /// <summary>
    /// The app engine settings.
    /// </summary>
    AppEngineSettings AppEngineSettings { get; }

    /// <summary>
    /// Updates the current settings.
    /// </summary>
    void Update();
  }
}
