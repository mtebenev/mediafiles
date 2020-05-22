using Microsoft.Extensions.Configuration;
using Mt.MediaFiles.AppEngine;
using System.IO.Abstractions;

namespace Mt.MediaFiles.ClientApp.Cli.Configuration
{
  /// <summary>
  /// Utility class keeping track of the settings source and able to update the settings.
  /// </summary>
  internal class AppSettingsManager
  {
    /// <summary>
    /// Ctor.
    /// </summary>
    private AppSettingsManager(AppSettings appSettings, AppEngineSettings appEngineSettings)
    {
      this.AppSettings = appSettings;
      this.AppEngineSettings = appEngineSettings;
    }

    /// <summary>
    /// The app settings.
    /// </summary>
    public AppSettings AppSettings { get; }

    /// <summary>
    /// The app engine settings.
    /// </summary>
    public AppEngineSettings AppEngineSettings { get; }

    /// <summary>
    /// The factory method.
    /// </summary>
    public static AppSettingsManager Create(IEnvironment environment, IFileSystem fileSystem)
    {
      var appEngineSettings = DefaultSettings.FillDefaultAppEngineSettings(environment, fileSystem);
      var environmentName = environment.GetEnvironmentVariable("MM_ENVIRONMENT");

      var configurationBuilder = new ConfigurationBuilder().AddEnvironmentVariables();

      string settingsPath = null;
      var testPath = fileSystem.Path.Combine(appEngineSettings.DataDirectory, $"appsettings.{environmentName}.json");
      if (fileSystem.File.Exists(testPath))
      {
        settingsPath = testPath;
      }

      if (string.IsNullOrEmpty(settingsPath))
      {
        testPath = fileSystem.Path.Combine(appEngineSettings.DataDirectory, "appsettings.json");
        if(fileSystem.File.Exists(testPath))
        {
          settingsPath = testPath;
        }
      }

      if (!string.IsNullOrEmpty(settingsPath))
      {
        var configStream = fileSystem.File.OpenRead(settingsPath);
        configurationBuilder.AddJsonStream(configStream);
      }

      var configuration = configurationBuilder.Build();
      var appSettings = configuration.Get<AppSettings>();
      appSettings = DefaultSettings.FillDefaultAppSettings(appSettings, environment, fileSystem);

      var result = new AppSettingsManager(appSettings, appEngineSettings);
      return result;
    }
  }
}
