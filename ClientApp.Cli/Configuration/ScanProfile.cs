namespace Mt.MediaFiles.ClientApp.Cli.Configuration
{
  /// <summary>
  /// Default scan profiles (configurations).
  /// </summary>
  internal enum ScanProfile
  {
    /// <summary>
    /// Normal scan (video imprints sub-task).
    /// </summary>
    Default,

    /// <summary>
    /// Quick scan (no sub-tasks, all file handlers).
    /// </summary>
    Quick,

    /// <summary>
    /// Full scan.
    /// Sub-tasks: video imprints, metadata scanning.
    /// All file handlers.
    /// </summary>
    Full
  }
}
