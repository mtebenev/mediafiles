namespace Mt.MediaMan.AppEngine.Scanning
{
  /// <summary>
  /// The info part saved for each scan root item. Contains original drive info
  /// </summary>
  public class InfoPartScanRoot : InfoPartBase
  {
    /// <summary>
    /// Original scan path
    /// </summary>
    public string RootPath { get; set; }

    public string DriveType { get; set; }

    /// <summary>
    /// IMAPI_MEDIA_PHYSICAL_TYPE
    /// </summary>
    public int MediaPhysicalType { get; set; }
  }
}
