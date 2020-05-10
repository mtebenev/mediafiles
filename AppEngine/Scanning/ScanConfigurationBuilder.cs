using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mt.MediaFiles.AppEngine.Scanning
{
  /// <summary>
  /// Builds the scan configuration according to required settings.
  /// </summary>
  internal class ScanConfigurationBuilder : IScanConfigurationBuilder
  {
    private readonly List<IScanService> _scanServices;

    public ScanConfigurationBuilder(IEnumerable<IScanService> scanServices)
    {
      this._scanServices = scanServices.ToList();
    }

    /// <summary>
    /// IScanConfigurationBuilder.
    /// </summary>
    public Task<IScanConfiguration> BuildAsync(ScanParameters scanParameters, MmConfig mmConfig)
    {
      var scanServices = scanParameters.ScanTaskIds
        .Select(id => this._scanServices.First(st => st.Id == id))
        .ToList();

      var configuration =
        new ScanConfiguration(scanParameters, mmConfig)
        {
          ScanServices = scanServices
        };

      return Task.FromResult<IScanConfiguration>(configuration);
    }
  }
}
