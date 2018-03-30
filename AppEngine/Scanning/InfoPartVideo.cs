namespace Mt.MediaMan.AppEngine.Scanning
{
  /// <summary>
  /// Info part containing information about video file
  /// </summary>
  public class InfoPartVideo : IInfoPart
  {
    public InfoPartVideo()
    {
      InfoPartId = "Video";
    }

    public string InfoPartId { get; set; }

    public int CatalogItemId { get; set; }
    public int VideoWidth { get; set; }
    public int VideoHeight { get; set; }
  }
}
