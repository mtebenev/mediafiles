namespace Mt.MediaFiles.AppEngine.Scanning
{
  /// <summary>
  /// Info part containing information about video file
  /// </summary>
  public class InfoPartVideo : InfoPartBase
  {
    public string Title { get; set; }

    public int VideoWidth { get; set; }
    public int VideoHeight { get; set; }

    /// <summary>
    /// Duration is in total milliseconds.
    /// </summary>
    public int Duration { get; set; }

    /// <summary>
    /// Short codec name (i.e. h264)
    /// </summary>
    public string VideoCodecName { get; set; }

    /// <summary>
    /// Full codec name (i.e. H.264 / AVC / MPEG-4 AVC / MPEG-4 part 10)
    /// </summary>
    public string VideoCodecLongName { get; set; }
  }
}
