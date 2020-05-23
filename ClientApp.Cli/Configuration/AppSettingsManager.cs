using Microsoft.Extensions.Configuration;
using Mt.MediaFiles.AppEngine;
using System.IO.Abstractions;
using System.Text.Json;

namespace Mt.MediaFiles.ClientApp.Cli.Configuration
{
  /// <summary>
  /// IAppSettingsManager implementation.
  /// </summary>
  internal class AppSettingsManager : IAppSettingsManager
  {
    private readonly IFileSystem _fileSystem;
    private readonly string _settingsFilePath;

    /// <summary>
    /// Ctor.
    /// </summary>
    private AppSettingsManager(IFileSystem fileSystem, AppSettings appSettings, AppEngineSettings appEngineSettings, string settingsFilePath)
    {
      this._fileSystem = fileSystem;
      this.AppSettings = appSettings;
      this.AppEngineSettings = appEngineSettings;
      this._settingsFilePath = settingsFilePath;
    }

    /// <summary>
    /// IAppSettingsManager.
    /// </summary>
    public AppSettings AppSettings { get; }

    /// <summary>
    /// IAppSettingsManager.
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

      var isNewConfig = true;
      if (!string.IsNullOrEmpty(settingsPath))
      {
        isNewConfig = false;
        var configStream = fileSystem.File.OpenRead(settingsPath);
        configurationBuilder.AddJsonStream(configStream);
      }
      else
      {
        settingsPath = fileSystem.Path.Combine(appEngineSettings.DataDirectory, "appsettings.json");
      }

      var configuration = configurationBuilder.Build();
      var appSettings = configuration.Get<AppSettings>();
      appSettings = DefaultSettings.FillDefaultAppSettings(appSettings, environment, fileSystem);

      var result = new AppSettingsManager(fileSystem, appSettings, appEngineSettings, settingsPath);
      if(isNewConfig)
      {
        result.Update();
      }

      return result;
    }

    /// <summary>
    /// IAppSettingsManager.
    /// </summary>
    public void Update()
    {
      var serialized = JsonSerializer.Serialize(
        this.AppSettings,
        new JsonSerializerOptions
        {
          PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
          WriteIndented = true
        }
      );
      this._fileSystem.File.WriteAllText(this._settingsFilePath, serialized);
    }
  }
}
