namespace Mt.MediaFiles.AppEngine.Matching
{
  /// <summary>
  /// File properties.
  /// Design note: seems like a very generic stuff but we use it nowhere except of the matching so far.
  /// </summary>
  public class FileProperties
  {
    public string Path { get; set; }
    public long Size { get; set; }
  }
}
