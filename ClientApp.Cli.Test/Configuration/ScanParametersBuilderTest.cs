using System.Collections.Generic;
using FluentAssertions;
using Mt.MediaFiles.ClientApp.Cli.Configuration;
using Xunit;

namespace Mt.MediaFiles.ClientApp.Cli.Test.Configuration
{
  public class ScanParametersBuilderTest
  {
    [Fact]
    public void Create_Default_Parameters()
    {
      var parameters = ScanParametersBuilder.Create("scan-path", "root-item", ScanProfile.Default);
      parameters.Should()
        .BeEquivalentTo(
        new
        {
          ScanPath = "scan-path",
          RootItemName = "root-item",
          FileHandlerIds = new[] { AppEngine.HandlerIds.FileHandlerVideo },
          ScanTaskIds = new[] { AppEngine.Video.HandlerIds.ScanSvcVideoImprints }
        });
    }

    [Fact]
    public void Create_Quick_Parameters()
    {
      var parameters = ScanParametersBuilder.Create("scan-path", "root-item", ScanProfile.Quick);
      parameters.Should()
        .BeEquivalentTo(
        new
        {
          ScanPath = "scan-path",
          RootItemName = "root-item",
          FileHandlerIds = new List<string>(),
          ScanTaskIds = new List<string>()
        });
    }

    [Fact]
    public void Create_Full_Parameters()
    {
      var parameters = ScanParametersBuilder.Create("scan-path", "root-item", ScanProfile.Full);
      parameters.Should()
        .BeEquivalentTo(
        new
        {
          ScanPath = "scan-path",
          RootItemName = "root-item",
          FileHandlerIds = new[] { AppEngine.HandlerIds.FileHandlerVideo },
          ScanTaskIds = new[] { AppEngine.Video.HandlerIds.ScanSvcVideoImprints, AppEngine.HandlerIds.ScanSvcScanInfo }
        });
    }
  }
}
