using System.Collections.Generic;
using Mt.MediaFiles.AppEngine.Scanning;
using Xunit;

namespace Mt.MediaFiles.AppEngine.Test.Scanning
{
  public class ScanConfigurationTest
  {
    [Fact]
    public void Should_Ignore_Files()
    {
      var paramters = new ScanParameters(
        @"x:\root_folder",
        "root_item",
        new List<string>(),
        new List<string>());

      var config = new MmConfig();
      config.Ignore = new[] { "Item1", "Item2" };


      var sut = new ScanConfiguration(paramters, config, new List<IScanServiceFactory>());

      Assert.True(sut.IsIgnoredEntry("Item2"));
      Assert.False(sut.IsIgnoredEntry("Item3"));
    }
  }
}
