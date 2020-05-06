using System.Linq;
using System.Threading.Tasks;
using AppEngine.Video.Comparison;
using AppEngine.Video.VideoImprint;
using Mt.MediaFiles.AppEngine.Video.VideoImprint;
using NSubstitute;
using Xunit;

namespace AppEngine.Video.Test.Comparison
{
  public class VideoComparerTest
  {
    [Fact]
    public async Task Compare_Identical_Imprints()
    {
      var mockStorage = Substitute.For<IVideoImprintStorage>();
      var mockBuilder = Substitute.For<IVideoImprintBuilder>();
      mockStorage.GetRecordsAsync(1).Returns(
        new[]
        {
          new VideoImprintRecord
          {
            CatalogItemId = 1,
            ImprintData = Enumerable.Range(0, 100).Select(x => (byte)100).ToArray(),
            VideoImprintId = 1
          }
        });
      mockStorage.GetRecordsAsync(2).Returns(
        new[]
        {
          new VideoImprintRecord
          {
            CatalogItemId = 2,
            ImprintData = Enumerable.Range(0, 100).Select(x => (byte)100).ToArray(),
            VideoImprintId = 2
          }
        });

      var comparer = new VideoComparer(mockStorage, mockBuilder);
      var result = await comparer.CompareItemsAsync(1, 2);

      Assert.True(result);
    }

    [Fact]
    public async Task Compare_Different_Imprints()
    {
      var mockBuilder = Substitute.For<IVideoImprintBuilder>();
      var mockStorage = Substitute.For<IVideoImprintStorage>();
      mockStorage.GetRecordsAsync(1).Returns(
        new[]
        {
          new VideoImprintRecord
          {
            CatalogItemId = 1,
            ImprintData = Enumerable.Range(0, 100).Select(x => (byte)0).ToArray(),
            VideoImprintId = 1
          }
        });
      mockStorage.GetRecordsAsync(2).Returns(
        new[]
        {
          new VideoImprintRecord
          {
            CatalogItemId = 2,
            ImprintData = Enumerable.Range(0, 100).Select(x => (byte)255).ToArray(),
            VideoImprintId = 2
          }
        });

      var comparer = new VideoComparer(mockStorage, mockBuilder);
      var result = await comparer.CompareItemsAsync(1, 2);

      Assert.False(result);
    }

    [Fact]
    public async Task Compare_Fs_Video()
    {
      var mockStorage = Substitute.For<IVideoImprintStorage>();
      mockStorage.GetRecordsAsync(1).Returns(
        new[]
        {
          new VideoImprintRecord
          {
            CatalogItemId = 1,
            ImprintData = Enumerable.Range(0, 100).Select(x => (byte)0).ToArray(),
            VideoImprintId = 1
          }
        });

      var mockBuilder = Substitute.For<IVideoImprintBuilder>();
      mockBuilder.CreateRecordAsync(0, @"x:\video_file").Returns(
          new VideoImprintRecord
          {
            CatalogItemId = 0,
            ImprintData = Enumerable.Range(0, 100).Select(x => (byte)255).ToArray(),
            VideoImprintId = 100
          }
      );

      var comparer = new VideoComparer(mockStorage, mockBuilder);
      var result = await comparer.CompareFsVideo(@"x:\video_file", 1);

      Assert.False(result);
    }
  }
}
