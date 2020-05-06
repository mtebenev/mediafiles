using System.Threading.Tasks;
using AppEngine.Video.Comparison;
using AppEngine.Video.VideoImprint;
using FluentAssertions;
using Mt.MediaFiles.AppEngine.Cataloging;
using Mt.MediaFiles.AppEngine.Video.Tasks;
using Mt.MediaFiles.TestUtils;
using NSubstitute;
using Xunit;

namespace Mt.MediaFiles.AppEngine.Video.Test.Tasks
{
  public class CatalogTaskFindVideoDuplicatesTest
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
      mockStorage.GetCatalogItemIdsAsync().Returns(new[] { 11, 12, 13, 14 });

      var mockCatalogContext = Substitute.For<ICatalogContext>();
      mockCatalogContext.Catalog.Returns(mockCatalog);

      var mockComparison = Substitute.For<IVideoComparer>();
      mockComparison.CompareItemsAsync(default, default).Returns(false);
      mockComparison.CompareItemsAsync(11, 13).Returns(true);
      mockComparison.CompareItemsAsync(12, 14).Returns(true);

      var mockFactory = Substitute.For<IVideoComparerFactory>();
      mockFactory.Create().Returns(mockComparison);

      var task = new CatalogTaskFindVideoDuplicates(mockStorage, mockFactory);
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
