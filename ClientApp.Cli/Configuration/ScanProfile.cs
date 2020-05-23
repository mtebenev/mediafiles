namespace Mt.MediaFiles.ClientApp.Cli.Configuration
{
  /// <summary>
  /// Default scan profiles (configurations).
  /// </summary>
  internal enum ScanProfile
  {
    /// <summary>
    /// Normal scan (video imprints service).
    /// </summary>
    Default,

    /// <summary>
    /// Quick scan (no scan services, all file handlers).
    /// </summary>
    Quick,

    /// <summary>
    /// All available scan services
    /// </summary>
    Full
  }
}
