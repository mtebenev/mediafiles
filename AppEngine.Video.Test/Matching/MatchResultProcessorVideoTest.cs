using FluentAssertions;
using Mt.MediaFiles.AppEngine.Matching;
using Mt.MediaFiles.AppEngine.Scanning;
using Mt.MediaFiles.AppEngine.Video.Matching;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mt.MediaFiles.AppEngine.Video.Test.Matching
{
  public class MatchResultProcessorVideoTest
  {
    [Fact]
    public async Task Produce_Output()
    {
      var mockBaseItemAccess = Substitute.For<IInfoPartAccess>();
      mockBaseItemAccess.GetFilePropertiesAsync(1).Returns(
        new FileProperties
        {
          Path = @"x:\folder1\file1.mp4",
          Size = 10
        });
      mockBaseItemAccess.GetInfoPartAsync<InfoPartVideo>(1).Returns(
        new InfoPartVideo
        {
          VideoWidth = 320,
          VideoHeight = 240,
          Duration = (int)TimeSpan.FromMinutes(1).TotalMilliseconds
        });

      var mockOtherItemAccess = Substitute.For<IInfoPartAccess>();
      mockOtherItemAccess.GetFilePropertiesAsync(2).Returns(
        new FileProperties
        {
          Path = @"x:\folder2\file1.mp4",
          Size = 200
        });
      mockOtherItemAccess.GetInfoPartAsync<InfoPartVideo>(2).Returns(
        new InfoPartVideo
        {
          VideoWidth = 640,
          VideoHeight = 480,
          Duration = (int)TimeSpan.FromMinutes(1).TotalMilliseconds
        });

      var matchGroup = new MatchResultGroup(1);
      matchGroup.AddItemId(2);

      var matchResult = new MatchResult(new List<MatchResultGroup> { matchGroup });
      var processor = new MatchResultProcessorVideo(mockBaseItemAccess, mockOtherItemAccess);


      var result = await processor.ProcessAsync(matchResult).ToListAsync();

      Assert.Single(result);

      result[0].Should().BeEquivalentTo(
          new
          {
            BaseItem = @"x:\folder1\file1.mp4",
            Items = new[]
            {
              new
              {
                Item = @"x:\folder2\file1.mp4",
                Properties = new[]
                {
                  new { Name = "file name", Value = "file1.mp4", RelativeValue = (string)null, Qualification = ComparisonQualification.Equal },
                  new { Name = "file size", Value = "200 B", RelativeValue = "+190 B", Qualification = ComparisonQualification.Worse },
                  new { Name = "length", Value = "00:01:00", RelativeValue = (string)null, Qualification = ComparisonQualification.Equal },
                  new { Name = "resolution", Value = "640x480", RelativeValue = (string)null, Qualification = ComparisonQualification.Better },
                }
              }
            }

          });
    }
  }
}
