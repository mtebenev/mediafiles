namespace Mt.MediaMan.AppEngine.Tools
{
  /// <summary>
  /// Contains summary information about a file related to a catalog item
  /// </summary>
  public class BriefFileInfo
  {
    public BriefFileInfo(int catalogItemId, string filePath, long fileSize)
    {
      CatalogItemId = catalogItemId;
      FilePath = filePath;
      FileSize = fileSize;
    }

    public int CatalogItemId { get; }
    public string FilePath { get; }
    public long FileSize { get; }
  }
}
