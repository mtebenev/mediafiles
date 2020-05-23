using Microsoft.Extensions.Configuration;
using Mt.MediaFiles.AppEngine;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
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

      var configurationBuilder = new ConfigurationBuilder().AddEnvironmentVariables();
      var configStreams = GetConfigStreams(environment, fileSystem, appEngineSettings);
      foreach(var cs in configStreams)
      {
        configurationBuilder.AddJsonStream(cs.stream);
      }

      var settingsPath = configStreams.Length > 0
        ? configStreams.Last().path
        : fileSystem.Path.Combine(appEngineSettings.DataDirectory, "appsettings.json");

      var configuration = configurationBuilder.Build();
      var appSettings = configuration.Get<AppSettings>();
      appSettings = DefaultSettings.FillDefaultAppSettings(appSettings, environment, fileSystem);

      var result = new AppSettingsManager(fileSystem, appSettings, appEngineSettings, settingsPath);

      // Initially update the settings if there is no settings file yet.
      if(configStreams.Length == 0)
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

    /// <summary>
    /// Looks for app configuration files:
    /// 1. Local to the executable
    /// 2. Data directory
    /// </summary>
    private static (string path, Stream stream)[] GetConfigStreams(IEnvironment environment, IFileSystem fileSystem, AppEngineSettings appEngineSettings)
    {
      var configFiles = new List<string> { "appsettings.json" };
      var mfEnvironmentName = environment.GetEnvironmentVariable("MF_ENVIRONMENT");
      if(!string.IsNullOrEmpty(mfEnvironmentName))
      {
        configFiles.Add($"appsettings.{mfEnvironmentName}.json");
      }

      var lookupDirectories = new[]
      {
        environment.GetBaseDirectory(),
        appEngineSettings.DataDirectory
      };

      List<string> foundConfigFiles = new List<string>();
      foreach(var d in lookupDirectories)
      {
        foreach(var fn in configFiles)
        {
          var configPath = fileSystem.Path.Combine(d, fn);
          if(fileSystem.File.Exists(configPath))
          {
            foundConfigFiles.Add(configPath);
          }
        }

        if(foundConfigFiles.Count > 0)
        {
          break;
        }
      }

      var result = foundConfigFiles
        .Select(path => (path: path, stream: fileSystem.File.OpenRead(path)))
        .ToArray();

      return result;
    }
  }
}
