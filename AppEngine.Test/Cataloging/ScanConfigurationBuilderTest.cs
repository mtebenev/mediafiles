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
      var mockSsFactories = new IScanServiceFactory[]
      {
        Substitute.For<IScanServiceFactory>(),
        Substitute.For<IScanServiceFactory>()
      };

      for(int i = 0; i < mockSsFactories.Length; i++)
      {
        var mockSs = Substitute.For<IScanService>();
        mockSs.Id.Returns($"svc{i}");
        mockSsFactories[i].Id.Returns($"svc{i}");
        mockSsFactories[i].Create().Returns(mockSs);
      }

      var builder = new ScanConfigurationBuilder(mockSsFactories);

      var scanParameters = new ScanParameters(
        @"x:\root_folder",
        "scan_root",
        new List<string> { "svc0", "svc1" },
        new List<string> ()
      );
      var mmConfig = new MmConfig();

      var configuration = await builder.BuildAsync(scanParameters, mmConfig);
      var services = configuration.CreateScanServices();

      Assert.Equal(services.Select(s => s.Id), new[] { "svc0", "svc1" });
    }

    [Fact]
    public async Task Create_Scan_Configuration_No_Scan_Services()
    {
      var mockSsFactories = new IScanServiceFactory[]
      {
        Substitute.For<IScanServiceFactory>(),
        Substitute.For<IScanServiceFactory>()
      };

      for(int i = 0; i < mockSsFactories.Length; i++)
      {
        var mockSs = Substitute.For<IScanService>();
        mockSs.Id.Returns($"svc{i}");
        mockSsFactories[i].Id.Returns($"svc{i}");
        mockSsFactories[i].Create().Returns(mockSs);
      }

      var builder = new ScanConfigurationBuilder(mockSsFactories);

      var scanParameters = new ScanParameters(
        @"x:\root_folder",
        "scan_root",
        new List<string>(),
        new List<string>()
      );
      var mmConfig = new MmConfig();

      var configuration = await builder.BuildAsync(scanParameters, mmConfig);
      var services = configuration.CreateScanServices();

      Assert.Empty(services);
    }

    [Fact]
    public async Task Should_Order_Scan_Services()
    {
      var mockSsFactories = Enumerable
        .Range(0, 3)
        .Select(i => Substitute.For<IScanServiceFactory>())
        .ToArray();

      for(int i = 0; i < mockSsFactories.Length; i++)
      {
        var mockSs = Substitute.For<IScanService>();
        mockSs.Id.Returns($"svc{i}");
        mockSsFactories[i].Id.Returns($"svc{i}");
        mockSsFactories[i].Create().Returns(mockSs);
      }

      mockSsFactories[0].Dependencies.Returns(new[] { "svc1" });
      mockSsFactories[1].Dependencies.Returns(new[] { "svc2" });
      mockSsFactories[2].Dependencies.Returns(new string[0]);

      var builder = new ScanConfigurationBuilder(mockSsFactories);

      var scanParameters = new ScanParameters(
        @"x:\root_folder",
        "scan_root",
        new List<string> { "svc0", "svc1", "svc2" },
        new List<string>()
      );
      var mmConfig = new MmConfig();

      var configuration = await builder.BuildAsync(scanParameters, mmConfig);
      var services = configuration.CreateScanServices();

      Assert.Equal(
        new[]
        {
          "svc2",
          "svc1",
          "svc0"
        },
        services.Select(ss => ss.Id));
    }
  }
}
