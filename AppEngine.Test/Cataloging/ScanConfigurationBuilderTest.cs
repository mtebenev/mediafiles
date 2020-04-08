using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediaToolkit.Services;
using Mt.MediaFiles.AppEngine.FileHandlers;
using Mt.MediaFiles.AppEngine.Scanning;
using Mt.MediaMan.AppEngine.Scanning;
using NSubstitute;
using Xunit;

namespace Mt.MediaFiles.AppEngine.Test.Cataloging
{
  public class ScanConfigurationBuilderTest
  {
    [Fact]
    public async Task Create_Scan_Configuration_All_Scan_Tasks()
    {
      var mockSp = Substitute.For<IServiceProvider>();
      mockSp.GetService(typeof(IMediaToolkitService)).Returns(Substitute.For<IMediaToolkitService>());

      var builder = new ScanConfigurationBuilder(mockSp);

      var scanParameters = new ScanParameters(
        @"x:\root_folder",
        "scan_root",
        new List<string> { HandlerIds.ScanTaskScanInfo },
        new List<string> { HandlerIds.FileHandlerVideo }
      );
      var mmConfig = new MmConfig();

      var configuration = await builder.BuildAsync(scanParameters, mmConfig);

      Assert.Equal(1, configuration.FileHandlers.Count);
      Assert.IsType<FileHandlerVideo>(configuration.FileHandlers[0]);

      Assert.Equal(1, configuration.ScanServices.Count);
      Assert.IsType<ScanServiceScanInfo>(configuration.ScanServices[0]);
    }

    [Fact]
    public async Task Create_Scan_Configuration_No_Scan_Tasks()
    {
      var mockSp = Substitute.For<IServiceProvider>();
      mockSp.GetService(typeof(IMediaToolkitService)).Returns(Substitute.For<IMediaToolkitService>());

      var builder = new ScanConfigurationBuilder(mockSp);

      var scanParameters = new ScanParameters(
        @"x:\root_folder",
        "scan_root",
        new List<string>(),
        new List<string>()
      );
      var mmConfig = new MmConfig();

      var configuration = await builder.BuildAsync(scanParameters, mmConfig);

      Assert.Equal(0, configuration.FileHandlers.Count);
      Assert.Equal(0, configuration.ScanServices.Count);
    }
  }
}
