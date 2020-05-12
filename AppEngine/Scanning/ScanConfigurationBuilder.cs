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

      scanServices.Sort(new SsDependencyComparer());

      var configuration =
        new ScanConfiguration(scanParameters, mmConfig)
        {
          ScanServices = scanServices
        };

      return Task.FromResult<IScanConfiguration>(configuration);
    }

    /// <summary>
    /// Orders scan services by dependencies.
    /// Note: no cycle check yet.
    /// </summary>
    private class SsDependencyComparer : IComparer<IScanService>
    {
      public int Compare(IScanService x, IScanService y)
      {
        return (y.Dependencies.Any(d => d == x.Id) && x.Dependencies.All(d => d != y.Id))
          ? -1
          : (x.Dependencies.Any(d => d == y.Id) && y.Dependencies.All(d => d != x.Id)) ? 1 : 0;
      }
    }
  }
}
