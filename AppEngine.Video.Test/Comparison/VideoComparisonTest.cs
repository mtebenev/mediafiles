using System.Threading.Tasks;
using AppEngine.Video.Comparison;
using AppEngine.Video.VideoImprint;
using NSubstitute;
using Xunit;

namespace AppEngine.Video.Test.Comparison
{
  public class VideoComparisonTest
  {
    [Fact]
    public async Task Compare_Imprints()
    {
      var mockStorage = Substitute.For<IVideoImprintStorage>();
      mockStorage.GetRecordsAsync(1).Returns(
        new[]
        {
          new VideoImprintRecord
          {
            CatalogItemId = 1,
            ImprintData = new byte[] { 1, 2, 3 },
            VideoImprintId = 1
          }
        });
      mockStorage.GetRecordsAsync(2).Returns(
        new[]
        {
          new VideoImprintRecord
          {
            CatalogItemId = 2,
            ImprintData = new byte[] { 1, 2, 3 },
            VideoImprintId = 2
          }
        });

      var comparison = new VideoComparison(mockStorage);
      var result = await comparison.CompareItemsAsync(1, 2);

      Assert.True(result);
    }
  }
}
