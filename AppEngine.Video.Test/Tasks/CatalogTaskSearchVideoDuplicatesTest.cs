using System.Linq;
using System.Threading.Tasks;
using AppEngine.Video.Comparison;
using AppEngine.Video.VideoImprint;
using FluentAssertions;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.Tasks;
using Mt.MediaFiles.AppEngine.Video.Tasks;
using Mt.MediaFiles.TestUtils;
using NSubstitute;
using Xunit;

namespace Mt.MediaFiles.AppEngine.Video.Test.Tasks
{
  public class CatalogTaskSearchVideoDuplicatesTest
  {
    [Fact]
    public async Task Should_Execute_Comparison_Tasks()
    {
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

      var mockStorage = Substitute.For<IVideoImprintStorage>();
      mockStorage.GetAllRecordsAsync().Returns(
        new[]
        {
          new VideoImprintRecord { CatalogItemId = 11, ImprintData = new byte[] { 1, 1, 1 } },
          new VideoImprintRecord { CatalogItemId = 12, ImprintData = new byte[] { 2, 2, 2 } },
          new VideoImprintRecord { CatalogItemId = 13, ImprintData = new byte[] { 3, 3, 3 } },
          new VideoImprintRecord { CatalogItemId = 14, ImprintData = new byte[] { 4, 4, 4 } },
        });

      var mockCatalogContext = Substitute.For<ICatalogContext>();
      mockCatalogContext.Catalog.Returns(mockCatalog);

      var mockComparer = Substitute.For<IVideoImprintComparer>();
      mockComparer.Compare(default, default).Returns(false);

      mockComparer.Compare(
        Arg.Is<byte[]>(a => a.SequenceEqual(new byte[]{ 1, 1, 1 })),
        Arg.Is<byte[]>(a => a.SequenceEqual(new byte[] { 3, 3, 3 })))
        .Returns(true);

      mockComparer.Compare(
        Arg.Is<byte[]>(a => a.SequenceEqual(new byte[] { 2, 2, 2 })),
        Arg.Is<byte[]>(a => a.SequenceEqual(new byte[] { 4, 4, 4 })))
        .Returns(true);

      var mockFactory = Substitute.For<IVideoImprintComparerFactory>();
      mockFactory.Create().Returns(mockComparer);
      var mockExecutionContext = Substitute.For<ITaskExecutionContext>();

      var task = new CatalogTaskSearchVideoDuplicates(mockExecutionContext, mockStorage, mockFactory);
      var result = await task.ExecuteTaskAsync(mockCatalogContext);

      var expectedResult = new[]
      {
        new
        {
          FileInfos = new[]
          {
            new { CatalogItemId = 11, FilePath = @"x:\root_folder\file1.mp4", FileSize = 10},
            new { CatalogItemId = 13, FilePath = @"x:\root_folder\file3.mp4", FileSize = 30},
          }
        },
        new
        {
          FileInfos = new[]
          {
            new { CatalogItemId = 12, FilePath = @"x:\root_folder\file2.mp4", FileSize = 20},
            new { CatalogItemId = 14, FilePath = @"x:\root_folder\file4.mp4", FileSize = 40},
          }
        }
      };

      result.Should().BeEquivalentTo(expectedResult);
    }
  }
}
