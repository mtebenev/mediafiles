namespace Mt.MediaFiles.AppEngine.Matching
{
  /// <summary>
  /// 1920x1080 is better than 320x240
  /// </summary>
  public enum ComparisonQualification
  {
    Worse,

    /// <summary>
    /// Example: equal file sizes
    /// </summary>
    Equal,

    /// <summary>
    /// Example: file name change.
    /// </summary>
    Neutral,

    Better
  }
}
