using System.Collections.Generic;
using System.Threading.Tasks;
using Mt.MediaFiles.AppEngine.Scanning;
using Mt.MediaMan.AppEngine.Scanning;
using NSubstitute;
using Xunit;

namespace Mt.MediaFiles.AppEngine.Test.Cataloging
{
  public class ScanConfigurationBuilderTest
  {
    [Fact]
    public async Task Create_Scan_Configuration_All_Scan_Services()
    {
      var mockScanServices = new List<IScanService>
      {
        Substitute.For<IScanService>(),
        Substitute.For<IScanService>()
      };

      mockScanServices[0].Id.Returns("svc1");
      mockScanServices[1].Id.Returns("svc2");

      var builder = new ScanConfigurationBuilder(mockScanServices);

      var scanParameters = new ScanParameters(
        @"x:\root_folder",
        "scan_root",
        new List<string> { "svc1", "svc2" },
        new List<string> ()
      );
      var mmConfig = new MmConfig();

      var configuration = await builder.BuildAsync(scanParameters, mmConfig);

      Assert.Equal(configuration.ScanServices, mockScanServices);
    }

    [Fact]
    public async Task Create_Scan_Configuration_No_Scan_Tasks()
    {
      var mockScanServices = new List<IScanService>
      {
        Substitute.For<IScanService>(),
        Substitute.For<IScanService>()
      };

      mockScanServices[0].Id.Returns("svc1");
      mockScanServices[1].Id.Returns("svc2");

      var builder = new ScanConfigurationBuilder(mockScanServices);

      var scanParameters = new ScanParameters(
        @"x:\root_folder",
        "scan_root",
        new List<string>(),
        new List<string>()
      );
      var mmConfig = new MmConfig();

      var configuration = await builder.BuildAsync(scanParameters, mmConfig);

      Assert.Equal(0, configuration.ScanServices.Count);
    }
  }
}
