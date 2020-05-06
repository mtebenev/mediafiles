using System.Threading.Tasks;

namespace AppEngine.Video.Comparison
{
  /// <summary>
  /// Mockable interface for the comparison.
  /// </summary>
  internal interface IVideoComparer
  {
    /// <summary>
    /// Executes the comparison between two catalog items.
    /// Returns true if the two videos are identical.
    /// </summary>
    Task<bool> CompareItemsAsync(int catalogItemId1, int catalogItem2);

    /// <summary>
    /// Executes the comparison between a video in FS and a cataloged video imprint.
    /// Returns true if the two videos are identical.
    /// </summary>
    Task<bool> CompareFsVideo(string fsPath, int catalogItemId);
  }

  public interface IVideoComparerFactory
  {
    internal IVideoComparer Create();
  }
}
