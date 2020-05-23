using MediaToolkit.Tasks;

namespace Mt.MediaFiles.AppEngine.Video.Thumbnail
{
  /// <summary>
  /// Calculates thumbnail size.
  /// </summary>
  internal static class ThumbnailSizeCalculator
  {
    public static FrameSize CalculateFrameSize(int videoWidth, int videoHeight)
    {
      var width = videoWidth;
      var height = videoHeight;
      while(height >> 1 > 200 || width >> 1 > 200)
      {
        width >>= 1;
        height >>= 1;
      }

      var result = new FrameSize(width, height);
      return result;
    }
  }
}
