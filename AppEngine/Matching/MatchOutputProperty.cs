namespace Mt.MediaFiles.AppEngine.Matching
{
  /// <summary>
  /// Property info in the output.
  /// </summary>
  public class MatchOutputProperty
  {
    /// <summary>
    /// Property name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The property value.
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// Better/worse.
    /// </summary>
    public ComparisonQualification Qualification { get; set; }
  }
}
