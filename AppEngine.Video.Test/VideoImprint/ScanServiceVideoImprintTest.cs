using System;
using System.Collections.Generic;
using System.IO;
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
    public async Task Save_Imprint_Record()
    {
      var imprintRecords = new[]
      {
        new VideoImprintRecord { CatalogItemId = 1 },
        new VideoImprintRecord { CatalogItemId = 2 }
      };

      var mockBuilder = Substitute.For<IVideoImprintBuilder>();
      mockBuilder.CreateRecordAsync(1, "file1.avi", Arg.Any<double>()).Returns(imprintRecords[0]);
      mockBuilder.CreateRecordAsync(2, "file2.avi", Arg.Any<double>()).Returns(imprintRecords[1]);

      var mockFs = Substitute.For<IFileSystem>();
      mockFs.Path.GetExtension(default).ReturnsForAnyArgs(x => Path.GetExtension((string)x[0]));

      var mockStorage = Substitute.For<IVideoImprintStorage>();
      var mockContext = Substitute.For<IScanServiceContext>();
      mockContext.GetItemData().Returns(CatalogItemDataMockBuilder.CreateVideoPart(1000));

      var service = new ScanServiceVideoImprint(mockFs, mockBuilder, mockStorage);
      await service.ScanAsync(mockContext, new CatalogItemRecord { CatalogItemId = 1, Path = "file1.avi" });
      await service.ScanAsync(mockContext, new CatalogItemRecord { CatalogItemId = 2, Path = "file2.avi" });

      await mockStorage.Received().SaveRecordAsync(imprintRecords[0]);
      await mockStorage.Received().SaveRecordAsync(imprintRecords[1]);
    }
  }
}
