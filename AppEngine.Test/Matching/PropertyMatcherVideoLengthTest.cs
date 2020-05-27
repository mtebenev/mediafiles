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
          RelativeValue = "+00:59:00",
          Qualification = ComparisonQualification.Better
        });
    }

    [Fact]
    public async Task Should_Handle_Negative_Diff()
    {
      var mockAccessBase = Substitute.For<IInfoPartAccess>();
      mockAccessBase.GetInfoPartAsync<InfoPartVideo>(0)
        .Returns(new InfoPartVideo
        {
          Duration = (int)TimeSpan.FromMinutes(10).TotalMilliseconds
        });
      var mockAccessOther = Substitute.For<IInfoPartAccess>();
      mockAccessOther.GetInfoPartAsync<InfoPartVideo>(0)
        .Returns(new InfoPartVideo
        {
          Duration = (int)TimeSpan.FromMinutes(9).TotalMilliseconds
        });

      var matcher = new PropertyMatcherVideoLength(mockAccessBase, mockAccessOther);
      var result = await matcher.MatchAsync(0, 0);

      result.Should().BeEquivalentTo(
        new MatchOutputProperty
        {
          Name = "length",
          Value = "00:09:00",
          RelativeValue = "-00:01:00",
          Qualification = ComparisonQualification.Worse
        });
    }


    [Fact]
    public async Task Should_Trim_Milliseconds()
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
          Duration = 112345
        });

      var matcher = new PropertyMatcherVideoLength(mockAccessBase, mockAccessOther);
      var result = await matcher.MatchAsync(0, 0);

      result.Should().BeEquivalentTo(
        new MatchOutputProperty
        {
          Name = "length",
          Value = "00:01:52",
          RelativeValue = "+00:00:52",
          Qualification = ComparisonQualification.Better
        });
    }

    [Fact]
    public async Task Should_Omit_Relative_Value_For_Equal_Lengths()
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
          Duration = (int)TimeSpan.FromMinutes(1).TotalMilliseconds
        });

      var matcher = new PropertyMatcherVideoLength(mockAccessBase, mockAccessOther);
      var result = await matcher.MatchAsync(0, 0);

      result.Should().BeEquivalentTo(
        new MatchOutputProperty
        {
          Name = "length",
          Value = "00:01:00",
          RelativeValue = null,
          Qualification = ComparisonQualification.Equal
        });
    }
  }
}
