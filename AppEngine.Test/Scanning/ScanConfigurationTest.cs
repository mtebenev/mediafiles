using System;
using System.Linq;
using System.Threading.Tasks;
using MediaToolkit.Services;
using Mt.MediaMan.AppEngine.FileHandlers;
using Mt.MediaMan.AppEngine.Scanning;
using NSubstitute;
using Xunit;

namespace Mt.MediaMan.AppEngine.Test.Scanning
{
  public class ScanConfigurationTest
  {
    [Fact]
    public void Should_Ignore_Files()
    {
      var mockServiceProvider = Substitute.For<IServiceProvider>();
      var config = new MmConfig();
      config.Ignore = new [] {"Item1", "Item2"};
      var sut = new ScanConfiguration("root_item", config, mockServiceProvider);

      Assert.True(sut.IsIgnoredEntry("Item2"));
      Assert.False(sut.IsIgnoredEntry("Item3"));
    }

    [Fact]
    public async Task Should_Create_File_Handlers()
    {
      var mockServiceProvider = Substitute.For<IServiceProvider>();
      mockServiceProvider.GetService(typeof(IMediaToolkitService)).Returns(Substitute.For<IMediaToolkitService>());
      var config = new MmConfig();
      var sut = new ScanConfiguration("root_item", config, mockServiceProvider);

      var fileHandlerTypes = sut.FileHandlers
        .Select(x => x.GetType())
        .ToArray();
      Assert.Equal(fileHandlerTypes, new[] { typeof(FileHandlerVideo), typeof(FileHandlerEpub) });
    }
  }
}
