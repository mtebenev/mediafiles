using Dapper.Contrib.Extensions;

namespace Mt.MediaMan.AppEngine.CatalogStorage
{
  [Table("CatalogItem")]
  internal class CatalogItemRecord
  {
    [Key]
    public int CatalogItemId { get; set; }

    /// <summary>
    /// File system name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Should be one of CatalogItemType strings
    /// </summary>
    public string ItemType { get; set; }

    /// <summary>
    /// Size in bytes for files
    /// </summary>
    public int Size { get; set; }

    public int ParentItemId { get; set; }

  }
}
