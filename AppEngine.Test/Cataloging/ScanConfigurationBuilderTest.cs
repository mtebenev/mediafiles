using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mt.MediaFiles.AppEngine.Scanning;
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

    [Fact]
    public async Task Should_Order_Scan_Services()
    {
      var mockScanServices = Enumerable
        .Range(0, 3)
        .Select(i => Substitute.For<IScanService>())
        .ToList();

      mockScanServices[0].Id.Returns("svc1");
      mockScanServices[0].Dependencies.Returns(new[] { "svc2" });
      mockScanServices[1].Id.Returns("svc2");
      mockScanServices[1].Dependencies.Returns(new[] { "svc3" });
      mockScanServices[2].Id.Returns("svc3");
      mockScanServices[2].Dependencies.Returns(new string[0]);

      var builder = new ScanConfigurationBuilder(mockScanServices);

      var scanParameters = new ScanParameters(
        @"x:\root_folder",
        "scan_root",
        new List<string> { "svc1", "svc2", "svc3" },
        new List<string>()
      );
      var mmConfig = new MmConfig();

      var configuration = await builder.BuildAsync(scanParameters, mmConfig);

      Assert.Equal(
        new[]
        {
          "svc3",
          "svc2",
          "svc1"
        },
        configuration.ScanServices.Select(ss => ss.Id));
    }
  }
}
