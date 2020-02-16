using Mt.MediaMan.AppEngine.Scanning;
using Xunit;

namespace Mt.MediaMan.AppEngine.Test.Scanning
{
  public class ScanConfigurationTest
  {
    [Fact]
    public void Should_Ignore_Files()
    {
      var config = new MmConfig();
      config.Ignore = new [] {"Item1", "Item2"};
      var sut = new ScanConfiguration("root_item", config);

      Assert.True(sut.IsIgnoredEntry("Item2"));
      Assert.False(sut.IsIgnoredEntry("Item3"));
    }
  }
}
