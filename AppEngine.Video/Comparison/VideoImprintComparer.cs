using Mt.MediaFiles.AppEngine;
using Mt.MediaFiles.AppEngine.Video.VideoImprint;

namespace AppEngine.Video.Comparison
{
  /// <summary>
  /// Performs video comparison for two catalog items.
  /// </summary>
  internal class VideoImprintComparer : IVideoImprintComparer
  {
    private readonly AHash _ahash;

    /// <summary>
    /// Ctor.
    /// </summary>
    public VideoImprintComparer()
    {
      this._ahash = new AHash(AppEngineConstants.ImprintThumbnailSize);
    }

    /// <summary>
    /// IVideoComparer.
    /// </summary>
    public bool Compare(byte[] videoImprintData1, byte[] videoImprintData2)
    {
      // Compare
      const int MarginDiff = 96; // Margin difference. If diff > maring => similar
      var similarity = this._ahash.ComputeSimilarity(videoImprintData1, videoImprintData2);
      var result = similarity >= MarginDiff;

      return result;
    }
  }
}
