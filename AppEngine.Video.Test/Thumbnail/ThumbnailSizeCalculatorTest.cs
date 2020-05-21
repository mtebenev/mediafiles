using Mt.MediaFiles.AppEngine.Video.Thumbnail;
using Xunit;

namespace Mt.MediaFiles.AppEngine.Video.Test.Thumbnail
{
  public class ThumbnailSizeCalculatorTest
  {
    [Fact]
    public void Calculate_Size()
    {
      var frameSize = ThumbnailSizeCalculator.CalculateFrameSize(1920, 1080);
      Assert.Equal(240, frameSize.Width);
      Assert.Equal(135, frameSize.Height);
    }
  }
}
