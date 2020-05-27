using FluentAssertions;
using Mt.MediaFiles.AppEngine.Matching;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace Mt.MediaFiles.AppEngine.Test.Matching
{
  public class PropertyMatcherFileNameTest
  {
    [Fact]
    public async Task Should_Produce_Neutral_Result_For_Different_File_Names()
    {
      var mockAccessBase = Substitute.For<IInfoPartAccess>();
      mockAccessBase.GetFilePropertiesAsync(0)
        .Returns(new FileProperties
        {
          Path = @"x:\dir1\file1.mp4"
        });
      var mockAccessOther = Substitute.For<IInfoPartAccess>();
      mockAccessOther.GetFilePropertiesAsync(0)
        .Returns(new FileProperties
        {
          Path = @"x:\dir2\file2.mp4"
        });

      var matcher = new PropertyMatcherFileName(mockAccessBase, mockAccessOther);
      var result = await matcher.MatchAsync(0, 0);

      result.Should().BeEquivalentTo(
        new MatchOutputProperty
        {
          Name = "file name",
          Value = "file2.mp4",
          Qualification = ComparisonQualification.Neutral
        });

    }
  }
}
