using FluentAssertions;
using Mt.MediaFiles.AppEngine.Matching;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace Mt.MediaFiles.AppEngine.Test.Matching
{
  public class PropertyMatcherFileSizeTest
  {
    [Fact]
    public async Task Compare_File_Size()
    {
      var mockAccessBase = Substitute.For<IInfoPartAccess>();
      mockAccessBase.GetFilePropertiesAsync(0)
        .Returns(new FileProperties
        {
          Path = "path-1",
          Size = 1000
        });
      var mockAccessOther = Substitute.For<IInfoPartAccess>();
      mockAccessOther.GetFilePropertiesAsync(0)
        .Returns(new FileProperties
        {
          Path = "path-2",
          Size = 100
        });

      var matcher = new PropertyMatcherFileSize(mockAccessBase, mockAccessOther);
      var result = await matcher.MatchAsync(0, 0);

      result.Should().BeEquivalentTo(
        new MatchOutputProperty
        {
          Name = "file size",
          Value = "100",
          Qualification = ComparisonQualification.Better
        });
    }
  }
}
