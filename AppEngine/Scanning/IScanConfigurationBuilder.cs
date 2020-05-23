using System.Threading.Tasks;

namespace Mt.MediaFiles.AppEngine.Scanning
{
  /// <summary>
  /// Builds scan configuration from scan parameters.
  /// </summary>
  internal interface IScanConfigurationBuilder
  {
    /// <summary>
    /// Builds the scan configuration from parameters.
    /// </summary>
    Task<IScanConfiguration> BuildAsync(ScanParameters scanParameters, MmConfig mmConfig);
  }
}
