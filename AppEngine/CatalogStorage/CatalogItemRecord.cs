namespace Mt.MediaMan.AppEngine.CatalogStorage
{
  public class CatalogItemRecord
  {
    public int CatalogItemId { get; set; }

    /// <summary>
    /// File system name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Size in bytes for files
    /// </summary>
    public int Size { get; set; }

    public int ParentItemId { get; set; }

  }
}
