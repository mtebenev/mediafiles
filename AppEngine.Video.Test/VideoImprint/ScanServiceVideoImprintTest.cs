using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using AppEngine.Video.VideoImprint;
using FluentAssertions;
using Mt.MediaFiles.AppEngine.CatalogStorage;
using Mt.MediaFiles.AppEngine.Scanning;
using Mt.MediaFiles.AppEngine.Video.VideoImprint;
using Mt.MediaFiles.TestUtils;
using NSubstitute;
using Xunit;

namespace Mt.MediaFiles.AppEngine.Video.Test.VideoImprint
{
  public class ScanServiceVideoImprintTest
  {
    [Fact]
    public async Task Should_Save_Records_On_Flush()
    {
      var mockBuilder = Substitute.For<IVideoImprintBuilder>();
      mockBuilder.CreateRecordAsync(1, "file1.avi", Arg.Any<double>()).Returns(new VideoImprintRecord { CatalogItemId = 1 });
      mockBuilder.CreateRecordAsync(2, "file2.avi", Arg.Any<double>()).Returns(new VideoImprintRecord { CatalogItemId = 2 });

      var mockFs = Substitute.For<IFileSystem>();
      var mockStorage = Substitute.For<IVideoImprintStorage>();
      var mockContext = Substitute.For<IScanServiceContext>();
      mockContext.GetItemData().Returns(CatalogItemDataMockBuilder.CreateVideoPart(1000));

      var service = new ScanServiceVideoImprint(mockFs, mockBuilder, mockStorage, 4);
      await service.ScanAsync(mockContext, new CatalogItemRecord { CatalogItemId = 1, Path = "file1.avi" });
      await service.ScanAsync(mockContext, new CatalogItemRecord { CatalogItemId = 2, Path = "file2.avi" });

      await mockStorage.DidNotReceiveWithAnyArgs().SaveRecordsAsync(default);

      IEnumerable<VideoImprintRecord> receivedRecords = null;
      await mockStorage.SaveRecordsAsync(Arg.Do<IEnumerable<VideoImprintRecord>>(r => { receivedRecords = r; }));

      await service.FlushAsync();

      receivedRecords
        .Select(r => r.CatalogItemId)
        .Should()
        .BeEquivalentTo(new[] { 1, 2 });
    }

    [Fact]
    public async Task Should_Flush_Intermediate_Blocks()
    {
      var mockBuilder = Substitute.For<IVideoImprintBuilder>();
      mockBuilder.CreateRecordAsync(1, "file1.avi", Arg.Any<double>()).Returns(new VideoImprintRecord { CatalogItemId = 1 });
      mockBuilder.CreateRecordAsync(2, "file2.avi", Arg.Any<double>()).Returns(new VideoImprintRecord { CatalogItemId = 2 });
      mockBuilder.CreateRecordAsync(3, "file3.avi", Arg.Any<double>()).Returns(new VideoImprintRecord { CatalogItemId = 3 });
      mockBuilder.CreateRecordAsync(4, "file4.avi", Arg.Any<double>()).Returns(new VideoImprintRecord { CatalogItemId = 4 });

      var mockFs = Substitute.For<IFileSystem>();
      var mockStorage = Substitute.For<IVideoImprintStorage>();
      var mockContext = Substitute.For<IScanServiceContext>();
      mockContext.GetItemData().Returns(CatalogItemDataMockBuilder.CreateVideoPart(1000));

      var service = new ScanServiceVideoImprint(mockFs, mockBuilder, mockStorage, 2);

      await service.ScanAsync(mockContext, new CatalogItemRecord { CatalogItemId = 1, Path = "file1.avi" });
      await service.ScanAsync(mockContext, new CatalogItemRecord { CatalogItemId = 2, Path = "file2.avi" });
      await service.ScanAsync(mockContext, new CatalogItemRecord { CatalogItemId = 3, Path = "file3.avi" });
      await service.ScanAsync(mockContext, new CatalogItemRecord { CatalogItemId = 4, Path = "file4.avi" });

      await mockStorage.ReceivedWithAnyArgs(1).SaveRecordsAsync(default);
      await service.FlushAsync();
      await mockStorage.ReceivedWithAnyArgs(2).SaveRecordsAsync(default);
    }
  }
}
