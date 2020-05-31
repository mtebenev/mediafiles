using System;
using System.Threading.Tasks;
using AppEngine.Video.VideoImprint;
using MediaToolkit.Model;
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
      var mockBuilder = Substitute.For<IVideoImprintBuilder>();

      var mockService = Substitute.For<IMediaToolkitService>();
      var ffProbeOutput = new FfProbeOutput
      {
        Format = new Format { Duration = TimeSpan.FromSeconds(10) }
      };
      var ffTaskResult = new GetMetadataResult(ffProbeOutput);
      mockService.ExecuteAsync<GetMetadataResult>(default)
        .ReturnsForAnyArgs(ffTaskResult);

      var thumbnailData = new byte[] { 1, 2, 3 };
      mockBuilder.CreateRecordAsync(10, @"x:\folder\file.mp4", TimeSpan.FromSeconds(5).TotalMilliseconds)
        .Returns(new VideoImprintRecord { CatalogItemId = 10, ImprintData = thumbnailData, VideoImprintId = 0 });

      var task = new VideoImprintUpdater(mockStorage, mockBuilder, mockService);
      await task.UpdateAsync(10, @"x:\folder\file.mp4");

      await mockStorage.Received().SaveRecordAsync(Arg.Is<VideoImprintRecord>(x =>
      x.CatalogItemId == 10
      && x.ImprintData == thumbnailData));
    }
  }
}
