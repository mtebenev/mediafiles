using System.Threading.Tasks;

namespace AppEngine.Video.Comparison
{
  /// <summary>
  /// Mockable interface for the comparison.
  /// </summary>
  internal interface IVideoComparison
  {
    /// <summary>
    /// Executes comparison. Returns true if the two videos are equal.
    /// </summary>
    Task<bool> CompareItemsAsync(int catalogItemId1, int catalogItem2);
  }

  public interface IVideoComparisonFactory
  {
    internal IVideoComparison Create();
  }
}
