using System.Collections.Generic;

namespace Mt.MediaFiles.AppEngine.Matching
{
  /// <summary>
  /// Media files search result.
  /// Design note: this is the core result containing item Ids. Usually we do search without slow catalog access (using indexes).
  /// On a higher level these IDs should be converted to catalog items or whatever structures with more information.
  /// </summary>
  public class MatchResult
  {
    /// <summary>
    /// Ctor.
    /// </summary>
    public MatchResult(List<MatchResultGroup> matchGroups)
    {
      this.MatchGroups = matchGroups;
    }

    /// <summary>
    /// Match groups.
    /// </summary>
    public List<MatchResultGroup> MatchGroups { get; }
  }
}
