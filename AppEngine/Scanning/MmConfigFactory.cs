using Microsoft.Extensions.Configuration;

namespace Mt.MediaFiles.AppEngine.Scanning
{
  /// <summary>
  /// Reads mm config for scanning.
  /// </summary>
  internal class MmConfigFactory
  {
    /// <summary>
    /// Loads the mmconfig.json in the scan directory.
    /// Always returns the config object (filled with defaults if the file is not founc).
    /// </summary>
    public static MmConfig LoadConfig(string path)
    {
      var configuration = new ConfigurationBuilder()
        .SetBasePath(path)
        .AddJsonFile("mmconfig.json", true)
        .Build();

      var mmConfig = configuration.Get<MmConfig>() ?? new MmConfig();
      return mmConfig;
    }
  }
}
