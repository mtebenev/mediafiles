using System.Threading.Tasks;
using AppEngine.Video.VideoImprint;
using MediaToolkit.Services;
using MediaToolkit.Tasks;
using Mt.MediaFiles.AppEngine.Video.VideoImprint;
using NSubstitute;
using Xunit;

namespace Mt.MediaFiles.AppEngine.Video.Test.VideoImprint
{
  public class VideoImprintUpdaterTest
  {
    [Fact]
    public async Task Create_Record()
    {
      var mockStorage = Substitute.For<IVideoImprintStorage>();
      var mockService = Substitute.For<IMediaToolkitService>();

      var thumbnailData = new byte[] { 1, 2, 3 };
      var thumbnailResult = new GetThumbnailResult(thumbnailData);
      mockService.ExecuteAsync<GetThumbnailResult>(default).ReturnsForAnyArgs(thumbnailResult);

      var task = new VideoImprintUpdater(mockStorage, mockService);
      await task.UpdateAsync(10, @"x:\folder\file.mp4");

      await mockStorage.Received().SaveRecordAsync(Arg.Is<VideoImprintRecord>(x =>
      x.CatalogItemId == 10
      && x.ImprintData == thumbnailData));
    }
  }
}
