using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppEngine.Video.VideoImprint;
using Mt.MediaFiles.AppEngine.Video.VideoImprint;

namespace AppEngine.Video.Comparison
{
  /// <summary>
  /// Performs video comparison for two catalog items.
  /// </summary>
  internal class VideoComparer : IVideoComparer
  {
    private readonly IVideoImprintStorage _storage;
    private readonly IVideoImprintBuilder _builder;

    /// <summary>
    /// Ctor.
    /// </summary>
    public VideoComparer(IVideoImprintStorage storage, IVideoImprintBuilder builder)
    {
      this._storage = storage;
      this._builder = builder;
    }

    /// <summary>
    /// IVideoComparer.
    /// </summary>
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
    /// IVideoComparer.
    /// </summary>
    public async Task<bool> CompareFsVideo(string fsPath, int catalogItemId)
    {
      var fsImprintRecords = await this.CreateFsVideoImprint(fsPath);
      var catalogImprintRecords = await this._storage.GetRecordsAsync(catalogItemId);

      // Compare
      const int MarginDiff = 90; // Margin difference. If diff > maring => similar
      const float minDiff = ((float)100 - MarginDiff) / 100;
      var diff = this.CalculateDifference(fsImprintRecords[0].ImprintData, catalogImprintRecords[0].ImprintData);
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

    /// <summary>
    /// Creates imprint for an FS video.
    /// </summary>
    private async Task<IList<VideoImprintRecord>> CreateFsVideoImprint(string fsPath)
    {
      var record = await this._builder.CreateRecordAsync(0, fsPath);
      return new List<VideoImprintRecord> { record };
    }
  }
}
