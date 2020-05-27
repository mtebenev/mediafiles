using System.Threading.Tasks;

namespace Mt.MediaFiles.AppEngine.Matching
{
  /// <summary>
  /// Matches a single property of two items for match output.
  /// </summary>
  public interface IPropertyMatcher
  {
    /// <summary>
    /// Matches a property of two items.
    /// </summary>
    Task<MatchOutputProperty> MatchAsync(int baseItemId, int otherItemId);
  }
}
