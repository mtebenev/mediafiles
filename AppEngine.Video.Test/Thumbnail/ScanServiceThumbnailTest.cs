using FluentAssertions;
using MediaToolkit.Services;
using MediaToolkit.Tasks;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using Mt.MediaFiles.AppEngine.Scanning;
using Mt.MediaFiles.AppEngine.Video.Thumbnail;
using Mt.MediaFiles.TestUtils;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mt.MediaFiles.AppEngine.Video.Test.Thumbnail
{
  public class ScanServiceThumbnailTest
  {
    /// <summary>
    /// Since we are taking 6 thumbnails (with equal maring at the start and at the end),
    /// the service should save 1 minute interval thumbnails for 8 minutes video.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task CreateThumbnails()
    {
      var duration = TimeSpan.FromMinutes(8);
      var mockFs = Substitute.For<IFileSystem>();
      mockFs.Path.GetExtension(default).ReturnsForAnyArgs(".mp4");

      var mockStorage = Substitute.For<IThumbnailStorage>();
      var mockContext = Substitute.For<IScanServiceContext>();
      mockContext.GetItemData().Returns(CatalogItemDataMockBuilder.CreateVideoPart((int)duration.TotalMilliseconds));

      var mockService = Substitute.For<IMediaToolkitService>();
      mockService.ExecuteAsync<GetThumbnailResult>(default).ReturnsForAnyArgs(new GetThumbnailResult(new byte[] { 1, 2, 3 }));

      var service = new ScanServiceThumbnail(mockFs, mockStorage, mockService);
      var record = new CatalogItemRecord { CatalogItemId = 100, ItemType = CatalogItemType.File };

      IList<ThumbnailRecord> resultRecords = null;
      mockStorage
        .WhenForAnyArgs(x => x.SaveRecordsAsync(default))
        .Do(r => resultRecords = r.Arg<IList<ThumbnailRecord>>());

      await service.ScanAsync(mockContext, record);

      // Verify
      Assert.All(resultRecords, r => Assert.Equal(100, r.CatalogItemId));
      resultRecords
        .Select(r => r.Offset)
        .Should()
        .BeEquivalentTo(new[] { 60000, 120000, 180000, 240000, 300000, 360000 });
    }
  }
}
