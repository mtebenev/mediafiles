namespace Mt.MediaMan.AppEngine.Scanning
{
  /// <summary>
  /// Info part containing information about video file
  /// </summary>
  public class InfoPartVideo : IInfoPart
  {
    public string InfoPartId => "Video";

    public int VideoWidth { get; set; }
    public int VideoHeight { get; set; }
  }
}
