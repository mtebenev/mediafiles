using FluentAssertions;
using Mt.MediaFiles.AppEngine.Matching;
using Mt.MediaFiles.AppEngine.Scanning;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Mt.MediaFiles.AppEngine.Test.Matching
{
  public class PropertyMatcherVideoLengthTest
  {
    [Fact]
    public async Task Compare_Video_Length()
    {
      var mockAccessBase = Substitute.For<IInfoPartAccess>();
      mockAccessBase.GetInfoPartAsync<InfoPartVideo>(0)
        .Returns(new InfoPartVideo
        {
          Duration = (int)TimeSpan.FromMinutes(1).TotalMilliseconds
        });
      var mockAccessOther = Substitute.For<IInfoPartAccess>();
      mockAccessOther.GetInfoPartAsync<InfoPartVideo>(0)
        .Returns(new InfoPartVideo
        {
          Duration = (int)TimeSpan.FromHours(1).TotalMilliseconds
        });

      var matcher = new PropertyMatcherVideoLength(mockAccessBase, mockAccessOther);
      var result = await matcher.MatchAsync(0, 0);

      result.Should().BeEquivalentTo(
        new MatchOutputProperty
        {
          Name = "length",
          Value = "01:00:00",
          Qualification = ComparisonQualification.Better
        });
    }
  }
}
