using AppEngine.Video.Comparison;
using AppEngine.Video.VideoImprint;
using FluentAssertions;
using MediaToolkit.Model;
using MediaToolkit.Services;
using MediaToolkit.Tasks;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.Tasks;
using Mt.MediaFiles.AppEngine.Video.Tasks;
using Mt.MediaFiles.AppEngine.Video.VideoImprint;
using Mt.MediaFiles.TestUtils;
using NSubstitute;
using System;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mt.MediaFiles.AppEngine.Video.Test.Tasks
{
  public class CatalogTaskSearchVideoTest
  {
    [Fact]
    public async Task Execute_Task()
    {
      var paths = new[]
      {
        @"x:\other-folder\another1.mp4",
        @"x:\other-folder\another2.mp4"
      };

      var catalogDef = @"
{
  path: 'Root',
  children: [
    {
      path: 'scan_root',
      rootPath: 'x:\\root_folder',
      children: [
        { id: 11, path: 'x:\\root_folder\\file1.mp4', fileSize: 10 },
        { id: 12, path: 'x:\\root_folder\\file2.mp4', fileSize: 20 },
        { id: 13, path: 'x:\\root_folder\\file3.mp4', fileSize: 30 },
        { id: 14, path: 'x:\\root_folder\\file4.mp4', fileSize: 40 }
      ]
    }
  ]
}
";

      var mockCatalog = CatalogMockBuilder.Create(catalogDef).Build();

      var mockCatalogContext = Substitute.For<ICatalogContext>();
      mockCatalogContext.Catalog.Returns(mockCatalog);

      var mockExecutionContext = Substitute.For<ITaskExecutionContext>();
      var mockStorage = Substitute.For<IVideoImprintStorage>();
      mockStorage.GetAllRecordsAsync().Returns(
        new[]
        {
          new VideoImprintRecord { CatalogItemId = 11, ImprintData = new byte[] { 1, 1, 1 } },
          new VideoImprintRecord { CatalogItemId = 12, ImprintData = new byte[] { 2, 2, 2 } },
          new VideoImprintRecord { CatalogItemId = 13, ImprintData = new byte[] { 3, 3, 3 } },
          new VideoImprintRecord { CatalogItemId = 14, ImprintData = new byte[] { 4, 4, 4 } },
        });

      var mockComparer = Substitute.For<IVideoImprintComparer>();
      mockComparer.Compare(default, default).ReturnsForAnyArgs(x =>
      {
        var x0 = (byte[])x[0];
        return x0.SequenceEqual((byte[])x[1]);
      });

      var mockFactory = Substitute.For<IVideoImprintComparerFactory>();
      mockFactory.Create().Returns(mockComparer);

      var mockService = Substitute.For<IMediaToolkitService>();
      mockService.ExecuteAsync<GetMetadataResult>(Arg.Any<FfTaskGetMetadata>())
        .Returns(new GetMetadataResult(new FfProbeOutput { Format = new Format { Duration = TimeSpan.FromMinutes(5) } }));

      var mockBuilder = Substitute.For<IVideoImprintBuilder>();
      mockBuilder.CreateRecordAsync(Arg.Any<int>(), @"x:\other-folder\another1.mp4", Arg.Any<double>())
        .Returns(new VideoImprintRecord { ImprintData =  new byte[] { 100, 100, 100 } });
      mockBuilder.CreateRecordAsync(Arg.Any<int>(), @"x:\other-folder\another2.mp4", Arg.Any<double>())
        .Returns(new VideoImprintRecord { ImprintData = new byte[] { 3, 3, 3} });

      var mockFs = Substitute.For<IFileSystem>();
      mockFs.Path.GetExtension(default).ReturnsForAnyArgs(x => Path.GetExtension((string)x[0]));

      var task = new CatalogTaskSearchVideo(mockExecutionContext, mockFs, mockStorage, mockFactory, mockBuilder, mockService, paths);
      var result = await task.ExecuteTaskAsync(mockCatalogContext);

      // Verify
      var expectedResult = new
      {
        MatchGroups = new[]
        {
          new
          {
            BaseItemId = 1,
            ItemIds = new[] { 13 }
          },
        }
      };

      result.Should().BeEquivalentTo(expectedResult);
    }
  }
}

