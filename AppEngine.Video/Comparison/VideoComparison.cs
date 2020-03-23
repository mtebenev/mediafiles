using System;
using System.Threading.Tasks;
using AppEngine.Video.VideoImprint;

namespace AppEngine.Video.Comparison
{
  /// <summary>
  /// Performs video comparison for two catalog items.
  /// </summary>
  internal class VideoComparison : IVideoComparison
  {
    private readonly IVideoImprintStorage _storage;

    /// <summary>
    /// Ctor.
    /// </summary>
    public VideoComparison(IVideoImprintStorage storage)
    {
      this._storage = storage;
    }

    public async Task<bool> CompareItemsAsync(int catalogItemId1, int catalogItemId2)
    {
      var imprintRecords1 = await this._storage.GetRecordsAsync(catalogItemId1);
      var imprintRecords2 = await this._storage.GetRecordsAsync(catalogItemId2);

      // Compare
      const int MarginDiff = 90; // Margin difference. If diff > maring => similar
      const float minDiff = ((float)100 - MarginDiff) / 100;
      var diff = this.CalculateDifference(imprintRecords1[0].ImprintData, imprintRecords2[0].ImprintData);
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
