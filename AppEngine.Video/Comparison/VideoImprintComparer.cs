using System;

namespace AppEngine.Video.Comparison
{
  /// <summary>
  /// Performs video comparison for two catalog items.
  /// </summary>
  internal class VideoImprintComparer : IVideoImprintComparer
  {
    /// <summary>
    /// IVideoComparer.
    /// </summary>
    public bool Compare(byte[] videoImprintData1, byte[] videoImprintData2)
    {
      // Compare
      const int MarginDiff = 90; // Margin difference. If diff > maring => similar
      const float minDiff = ((float)100 - MarginDiff) / 100;
      var diff = this.CalculateDifference(videoImprintData1, videoImprintData2);
      var result = diff < minDiff;

      return result;
    }

    /// <summary>
    /// Calculates the difference between two imprints.
    /// </summary>
    private float CalculateDifference(byte[] imprintData1, byte[] imprintData2)
    {
      if(imprintData1.Length != imprintData2.Length)
        throw new ArgumentException("Video imprints have different size.");

      long diff = 0;
      for(var y = 0; y < imprintData1.Length; y++)
      {
        diff += Math.Abs(imprintData1[y] - imprintData2[y]);
      }
      return (float)diff / imprintData1.Length / 256;
    }
  }
}
