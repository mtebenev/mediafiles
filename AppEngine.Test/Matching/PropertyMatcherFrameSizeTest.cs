using FluentAssertions;
using Mt.MediaFiles.AppEngine.Matching;
using Mt.MediaFiles.AppEngine.Scanning;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace Mt.MediaFiles.AppEngine.Test.Matching
{
  public class PropertyMatcherFrameSizeTest
  {
    [Fact]
    public async Task Compare_Frame_Size()
    {
      var mockAccessBase = Substitute.For<IInfoPartAccess>();
      mockAccessBase.GetInfoPartAsync<InfoPartVideo>(0)
        .Returns(new InfoPartVideo
        {
          VideoWidth = 320,
          VideoHeight = 240
        });
      var mockAccessOther = Substitute.For<IInfoPartAccess>();
      mockAccessOther.GetInfoPartAsync<InfoPartVideo>(0)
        .Returns(new InfoPartVideo
        {
          VideoWidth = 1024,
          VideoHeight = 768
        });

      var matcher = new PropertyMatcherFrameSize(mockAccessBase, mockAccessOther);
      var result = await matcher.MatchAsync(0, 0);

      result.Should().BeEquivalentTo(
        new MatchOutputProperty
        {
          Name = "resolution",
          Value = "1024x768",
          Qualification = ComparisonQualification.Better
        });
    }
  }
}
