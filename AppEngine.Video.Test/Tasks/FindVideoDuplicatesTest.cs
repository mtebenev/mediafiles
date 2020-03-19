using System.Threading.Tasks;
using AppEngine.Video.Comparison;
using AppEngine.Video.VideoImprint;
using FluentAssertions;
using Mt.MediaMan.AppEngine.Cataloging;
using Mt.MediaMan.AppEngine.Tasks;
using Mt.MediaMan.AppEngine.Test.TestUtils;
using NSubstitute;
using Xunit;

namespace AppEngine.Video.Test.Tasks
{
  public class FindVideoDuplicatesTest
  {
    [Fact]
    public async Task Should_Execute_Comparison_Tasks()
    {
      var catalogDef = @"
{
  name: 'Root',
  children: [
    {
      name: 'root_folder',
      rootPath: 'x:\\root_folder',
      children: [
        {
          name: 'file1.mp4',
          id: 11,
          fileSize: 10
        },
        {
          name: 'file2.mp4',
          id: 12,
          fileSize: 20
        },
        {
          name: 'file3.mp4',
          id: 13,
          fileSize: 30
        },
        {
          name: 'file4.mp4',
          id: 14,
          fileSize: 40
        }
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

      var mockComparison = Substitute.For<IVideoComparison>();
      mockComparison.CompareItemsAsync(default, default).Returns(false);
      mockComparison.CompareItemsAsync(11, 13).Returns(true);
      mockComparison.CompareItemsAsync(12, 14).Returns(true);

      var mockFactory = Substitute.For<IVideoComparisonFactory>();
      mockFactory.Create().Returns(mockComparison);

      var task = new FindVideoDuplicates(mockStorage, mockFactory);
      var result = await task.ExecuteAsync(mockCatalogContext);

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
