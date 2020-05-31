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
    private readonly List<IScanServiceFactory> _scanServiceFactories;

    public ScanConfigurationBuilder(IEnumerable<IScanServiceFactory> scanServiceFactories)
    {
      this._scanServiceFactories = scanServiceFactories.ToList();
    }

    /// <summary>
    /// IScanConfigurationBuilder.
    /// </summary>
    public Task<IScanConfiguration> BuildAsync(ScanParameters scanParameters, MmConfig mmConfig)
    {
      var ssFactories = scanParameters.ScanSvcIds
        .Select(id => this._scanServiceFactories.First(st => st.Id == id))
        .ToList();

      ssFactories.Sort(new SsDependencyComparer());

      var configuration = new ScanConfiguration(scanParameters, mmConfig, ssFactories);
      return Task.FromResult<IScanConfiguration>(configuration);
    }

    /// <summary>
    /// Orders scan services by dependencies.
    /// Note: no cycle check yet.
    /// </summary>
    private class SsDependencyComparer : IComparer<IScanServiceFactory>
    {
      public int Compare(IScanServiceFactory x, IScanServiceFactory y)
      {
        return (y.Dependencies.Any(d => d == x.Id) && x.Dependencies.All(d => d != y.Id))
          ? -1
          : (x.Dependencies.Any(d => d == y.Id) && y.Dependencies.All(d => d != x.Id)) ? 1 : 0;
      }
    }
  }
}
