using System.Threading.Tasks;
using AppEngine.Video.VideoImprint;
using FluentAssertions;
using MediaToolkit.Services;
using MediaToolkit.Tasks;
using Mt.MediaFiles.AppEngine.Video.VideoImprint;
using NSubstitute;
using Xunit;

namespace Mt.MediaFiles.AppEngine.Video.Test.VideoImprint
{
  public class VideoImprintBuilderTest
  {
    [Fact]
    public async Task Create_Record_From_File()
    {
      var mockService = Substitute.For<IMediaToolkitService>();
      mockService.ExecuteAsync<GetThumbnailResult>(default).ReturnsForAnyArgs(
        new GetThumbnailResult(new byte[] { 1, 2, 3 }));

      var builder = new VideoImprintBuilder(mockService);
      var record = await builder.CreateRecordAsync(100, @"x:\folder\file.mp4");

      record.Should().BeEquivalentTo(
        new VideoImprintRecord
        {
          CatalogItemId = 100,
          ImprintData = new byte[] { 1, 2, 3 },
          VideoImprintId = 0
        });
    }
  }
}
