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
      var parameters = ScanParametersBuilder.Create("scan-path", "root-item");
      parameters.Should()
        .BeEquivalentTo(
        new
        {
          ScanPath = "scan-path",
          RootItemName = "root-item",
          FileHandlerIds = new[] { AppEngine.HandlerIds.FileHandlerVideo },
          ScanTaskIds = new[] { AppEngine.Video.HandlerIds.ScanTaskVideoImprints }
        });
    }

    [Fact]
    public void Create_Quick_Parameters()
    {
      var parameters = ScanParametersBuilder.CreateQuick("scan-path", "root-item");
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
      var parameters = ScanParametersBuilder.CreateFull("scan-path", "root-item");
      parameters.Should()
        .BeEquivalentTo(
        new
        {
          ScanPath = "scan-path",
          RootItemName = "root-item",
          FileHandlerIds = new[] { AppEngine.HandlerIds.FileHandlerVideo },
          ScanTaskIds = new[] { AppEngine.Video.HandlerIds.ScanTaskVideoImprints, AppEngine.HandlerIds.ScanTaskScanInfo }
        });
    }
  }
}
