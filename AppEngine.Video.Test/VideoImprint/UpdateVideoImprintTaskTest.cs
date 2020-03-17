using System.Threading.Tasks;
using AppEngine.Video.VideoImprint;
using MediaToolkit.Services;
using MediaToolkit.Tasks;
using Mt.MediaMan.AppEngine.Cataloging;
using NSubstitute;
using Xunit;

namespace AppEngine.Video.Test.VideoImprint
{
  public class UpdateVideoImprintTaskTest
  {
    [Fact]
    public async Task Create_Record()
    {
      var mockStorage = Substitute.For<IVideoImprintStorage>();
      var mockService = Substitute.For<IMediaToolkitService>();

      var thumbnailData = new byte[] { 1, 2, 3 };
      var thumbnailResult = new GetThumbnailResult(thumbnailData);
      mockService.ExecuteAsync<GetThumbnailResult>(default).ReturnsForAnyArgs(thumbnailResult);
      var mockCi = Substitute.For<ICatalogItem>();
      mockCi.CatalogItemId.Returns(10);

      var task = new UpdateVideoImprintTask(mockStorage, mockService, mockCi, @"x:\folder\file.mp4");
      await task.ExecuteAsync();

      await mockStorage.Received().SaveRecordAsync(Arg.Is<VideoImprintRecord>(x =>
      x.CatalogItemId == 10
      && x.ImprintData == thumbnailData));
    }
  }
}
