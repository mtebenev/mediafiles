namespace Mt.MediaFiles.AppEngine.Scanning
{
  /// <summary>
  /// Deserialized MM scan configuration from .mmconfig.json files
  /// </summary>
  internal class MmConfig
  {
    public MmConfig()
    {
      this.Ignore = new string[] { };
    }

    /// <summary>
    /// FS names to ignore. For now this works only for media roots.
    /// </summary>
    public string[] Ignore { get; set; }
  }
}
