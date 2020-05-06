using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppEngine.Video.VideoImprint;
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

      var thumbnailData = new byte[] { 1, 2, 3 };
      mockBuilder.CreateRecordAsync(10, @"x:\folder\file.mp4")
        .Returns(new VideoImprintRecord { CatalogItemId = 10, ImprintData = thumbnailData, VideoImprintId = 0 });

      var task = new VideoImprintUpdater(mockStorage, mockBuilder);
      await task.UpdateAsync(10, @"x:\folder\file.mp4");

      await mockStorage.Received().SaveRecordsAsync(Arg.Is<IEnumerable<VideoImprintRecord>>(x =>
      x.First().CatalogItemId == 10
      && x.First().ImprintData == thumbnailData
      && x.Count() == 1));
    }
  }
}
